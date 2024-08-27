using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.CitiesTests;

public class GetAllCitiesFromCountryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllCitiesFromCountryHandler _handler;

    public GetAllCitiesFromCountryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllCitiesFromCountryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedCities = CityFakers.CreateCityResponseDto().Generate(3);
        var cacheKey = "country:1:cities";
        
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CityResponseDto>>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedCities);

        var result = await _handler.Handle(new GetAllCitiesFromCountryQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedCities);
        _unitOfWorkMock.Verify(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CountryRepository.GetAllCitiesFromCountryAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var country = CountryFakers.CreateCountry().Generate();
        var citiesFromRepo = CityFakers.CreateCity().Generate(3);
        var mappedCities = CityFakers.CreateCityResponseDto().Generate(3);
        var cacheKey = "country:1:cities";

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CityResponseDto>>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<CityResponseDto>);
        _unitOfWorkMock.Setup(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(country);
        _unitOfWorkMock.Setup(u => u.CountryRepository.GetAllCitiesFromCountryAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(citiesFromRepo);
        _mapperMock.Setup(m => m.Map<List<CityResponseDto>>(citiesFromRepo))
            .Returns(mappedCities);

        var result = await _handler.Handle(new GetAllCitiesFromCountryQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(mappedCities);
        _unitOfWorkMock.Verify(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CountryRepository.GetAllCitiesFromCountryAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, mappedCities, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCountryDoesNotExist()
    {
        var cacheKey = "country:1:cities";
        
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CityResponseDto>>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<CityResponseDto>);
        _unitOfWorkMock.Setup(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Country);

        Func<Task> act = async () => await _handler.Handle(new GetAllCitiesFromCountryQuery(1), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CountryRepository.GetAllCitiesFromCountryAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}
