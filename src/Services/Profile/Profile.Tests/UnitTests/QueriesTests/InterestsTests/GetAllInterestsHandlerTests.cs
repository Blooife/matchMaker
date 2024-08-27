using AutoMapper;
using FluentAssertions;
using Moq;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.UseCases.InterestUseCases.Queries.GetAll;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.InterestsTests;

public class GetAllInterestsHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllInterestsHandler _handler;

    public GetAllInterestsHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllInterestsHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedInterests = InterestFakers.CreateInterestResponseDto().Generate(2);
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<InterestResponseDto>>("interests", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedInterests);

        var result = await _handler.Handle(new GetAllInterestsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedInterests);
        _unitOfWorkMock.Verify(u => u.InterestRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var interestsFromRepo = InterestFakers.CreateInterest().Generate(2);
        var countryDtos = InterestFakers.CreateInterestResponseDto().Generate(2);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<InterestResponseDto>>("interests", It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<InterestResponseDto>);
        _unitOfWorkMock.Setup(u => u.InterestRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(interestsFromRepo);
        _mapperMock.Setup(m => m.Map<List<InterestResponseDto>>(interestsFromRepo))
            .Returns(countryDtos);

        var result = await _handler.Handle(new GetAllInterestsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(countryDtos);
        _unitOfWorkMock.Verify(u => u.InterestRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("interests", countryDtos, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}