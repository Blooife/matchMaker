using Moq;
using AutoMapper;
using Profile.Application.DTOs.City.Response;
using Profile.Application.UseCases.CityUseCases.Queries.GetAll;
using FluentAssertions;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.CitiesTests;

public class GetAllCitiesHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllCitiesHandler _handler;

    public GetAllCitiesHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllCitiesHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedCities = CityFakers.CreateCityResponseDto().Generate(2);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CityResponseDto>>("cities", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedCities);

        var result = await _handler.Handle(new GetAllCitiesQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedCities);
        _unitOfWorkMock.Verify(u => u.CityRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var citiesFromRepo = CityFakers.CreateCity().Generate(2);
        var cityDtos = CityFakers.CreateCityResponseDto().Generate(2);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CityResponseDto>>("cities", It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<CityResponseDto>);
        _unitOfWorkMock.Setup(u => u.CityRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(citiesFromRepo);
        _mapperMock.Setup(m => m.Map<List<CityResponseDto>>(citiesFromRepo))
            .Returns(cityDtos);

        var result = await _handler.Handle(new GetAllCitiesQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cityDtos);
        _unitOfWorkMock.Verify(u => u.CityRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("cities", cityDtos, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}