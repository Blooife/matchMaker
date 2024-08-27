using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.Country.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.CountryUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.CountriesTests;

public class GetCountryByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetCountryByIdHandler _handler;

    public GetCountryByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetCountryByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cachedCountry = CountryFakers.CreateCountryResponseDto().Generate();
        var cacheKey = "country:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<CountryResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedCountry);

        var result = await _handler.Handle(new GetCountryByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedCountry);
        _unitOfWorkMock.Verify(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var countryFromRepo = CountryFakers.CreateCountry().Generate();
        var countryDto = CountryFakers.CreateCountryResponseDto().Generate();
        var cacheKey = "country:1";

        _cacheServiceMock.Setup(c => c.GetAsync<CountryResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CountryResponseDto);
        _unitOfWorkMock.Setup(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(countryFromRepo);
        _mapperMock.Setup(m => m.Map<CountryResponseDto>(countryFromRepo))
            .Returns(countryDto);

        var result = await _handler.Handle(new GetCountryByIdQuery(1), CancellationToken.None);

        result.Should().BeEquivalentTo(countryDto);
        _unitOfWorkMock.Verify(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, countryDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCountryDoesNotExist()
    {
        var cacheKey = "country:1";
        
        _cacheServiceMock.Setup(c => c.GetAsync<CountryResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CountryResponseDto);
        _unitOfWorkMock.Setup(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Country);

        Func<Task> act = async () => await _handler.Handle(new GetCountryByIdQuery(1), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.CountryRepository.FirstOrDefaultAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<object>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}