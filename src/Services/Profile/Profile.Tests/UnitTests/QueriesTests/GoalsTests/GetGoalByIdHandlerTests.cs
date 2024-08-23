using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Goal.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.GoalUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.GoalsTests;

public class GetGoalByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetGoalByIdHandler _handler;

    public GetGoalByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetGoalByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedGoal = GoalFakers.CreateGoalResponseDto().Generate();
        var cacheKey = "goal:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<GoalResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedGoal);

        var result = await _handler.Handle(new GetGoalByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedGoal);
        _unitOfWorkMock.Verify(u => u.GoalRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var goalFromRepo = GoalFakers.CreateGoal().Generate();
        var goalDto = GoalFakers.CreateGoalResponseDto().Generate();
        var cacheKey = "goal:1";

        _cacheServiceMock.Setup(c => c.GetAsync<GoalResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as GoalResponseDto);
        _unitOfWorkMock.Setup(u => u.GoalRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(goalFromRepo);
        _mapperMock.Setup(m => m.Map<GoalResponseDto>(goalFromRepo))
            .Returns(goalDto);

        var result = await _handler.Handle(new GetGoalByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(goalDto);
        _unitOfWorkMock.Verify(u => u.GoalRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, goalDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenGoalDoesNotExist()
    {
        var cacheKey = "goal:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<GoalResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as GoalResponseDto);
        _unitOfWorkMock.Setup(u => u.GoalRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Goal);

        Func<Task> act = async () => await _handler.Handle(new GetGoalByIdQuery(1), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.GoalRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}