using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.InterestUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.InterestsTests;

public class GetInterestByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetInterestByIdHandler _handler;

    public GetInterestByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetInterestByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedInterest = InterestFakers.CreateInterestResponseDto().Generate();
        var cacheKey = "interest:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<InterestResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedInterest);

        var result = await _handler.Handle(new GetInterestByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedInterest);
        _unitOfWorkMock.Verify(u => u.InterestRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var interestFromRepo = InterestFakers.CreateInterest().Generate();
        var interestDto = InterestFakers.CreateInterestResponseDto().Generate();
        var cacheKey = "interest:1";

        _cacheServiceMock.Setup(c => c.GetAsync<InterestResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as InterestResponseDto);
        _unitOfWorkMock.Setup(u => u.InterestRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(interestFromRepo);
        _mapperMock.Setup(m => m.Map<InterestResponseDto>(interestFromRepo))
            .Returns(interestDto);

        var result = await _handler.Handle(new GetInterestByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(interestDto);
        _unitOfWorkMock.Verify(u => u.InterestRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, interestDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenInterestDoesNotExist()
    {
        var cacheKey = "interest:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<InterestResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as InterestResponseDto);
        _unitOfWorkMock.Setup(u => u.InterestRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Interest);

        Func<Task> act = async () => await _handler.Handle(new GetInterestByIdQuery(1), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.InterestRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}