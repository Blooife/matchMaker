using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Goal.Response;
using Profile.Application.UseCases.GoalUseCases.Queries.GetAll;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.GoalsTests;

public class GetAllGoalsHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllGoalsHandler _handler;

    public GetAllGoalsHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllGoalsHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedGoals = GoalFakers.CreateGoalResponseDto().Generate(2);
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<GoalResponseDto>>("goals", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedGoals);

        var result = await _handler.Handle(new GetAllGoalsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedGoals);
        _unitOfWorkMock.Verify(u => u.GoalRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var goalsFromRepo = GoalFakers.CreateGoal().Generate(2);
        var countryDtos = GoalFakers.CreateGoalResponseDto().Generate(2);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<GoalResponseDto>>("goals", It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<GoalResponseDto>);
        _unitOfWorkMock.Setup(u => u.GoalRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(goalsFromRepo);
        _mapperMock.Setup(m => m.Map<List<GoalResponseDto>>(goalsFromRepo))
            .Returns(countryDtos);

        var result = await _handler.Handle(new GetAllGoalsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(countryDtos);
        _unitOfWorkMock.Verify(u => u.GoalRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("goals", countryDtos, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}