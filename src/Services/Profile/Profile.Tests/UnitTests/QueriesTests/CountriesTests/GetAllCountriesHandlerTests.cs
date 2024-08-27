using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.Country.Response;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAll;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.CountriesTests;

public class GetAllCountriesHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetAllCountriesHandler _handler;

    public GetAllCountriesHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetAllCountriesHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedCountries = CountryFakers.CreateCountryResponseDto().Generate(2);
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CountryResponseDto>>("countries", It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedCountries);

        var result = await _handler.Handle(new GetAllCountriesQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedCountries);
        _unitOfWorkMock.Verify(u => u.CountryRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var countriesFromRepo = CountryFakers.CreateCountry().Generate(2);
        var countryDtos = CountryFakers.CreateCountryResponseDto().Generate(2);

        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<CountryResponseDto>>("countries", It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as IEnumerable<CountryResponseDto>);
        _unitOfWorkMock.Setup(u => u.CountryRepository.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(countriesFromRepo);
        _mapperMock.Setup(m => m.Map<List<CountryResponseDto>>(countriesFromRepo))
            .Returns(countryDtos);

        var result = await _handler.Handle(new GetAllCountriesQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(countryDtos);
        _unitOfWorkMock.Verify(u => u.CountryRepository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync("countries", countryDtos, null, It.IsAny<CancellationToken>()), Times.Once);
    }
}