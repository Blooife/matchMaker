using Moq;
using AutoMapper;
using FluentAssertions;
using Profile.Application.DTOs.City.Response;
using Profile.Application.Exceptions;
using Profile.Application.UseCases.CityUseCases.Queries.GetById;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Tests.Fakers;

namespace Profile.Tests.UnitTests.QueriesTests.CitiesTests;

public class GetCityByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GetCityByIdHandler _handler;

    public GetCityByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _cacheServiceMock = new Mock<ICacheService>();
        _handler = new GetCityByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object, _cacheServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnCachedData_WhenCacheIsNotNull()
    {
        var cityId = 1;
        var cachedCity = CityFakers.CreateCityResponseDto().Generate();
        var cacheKey = $"city:{cityId}";

        _cacheServiceMock.Setup(c => c.GetAsync<CityResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cachedCity);

        var result = await _handler.Handle(new GetCityByIdQuery(cityId), CancellationToken.None);

        result.Should().BeEquivalentTo(cachedCity);
        _unitOfWorkMock.Verify(u => u.CityRepository.FirstOrDefaultAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<CityResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnDataFromRepository_WhenCacheIsNull()
    {
        var cityId = 1;
        var city = CityFakers.CreateCity().Generate();
        var cityDto = CityFakers.CreateCityResponseDto().Generate();
        var cacheKey = $"city:{cityId}";

        _cacheServiceMock.Setup(c => c.GetAsync<CityResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CityResponseDto);
        _unitOfWorkMock.Setup(u => u.CityRepository.FirstOrDefaultAsync(cityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(city);
        _mapperMock.Setup(m => m.Map<CityResponseDto>(city))
            .Returns(cityDto);

        var result = await _handler.Handle(new GetCityByIdQuery(cityId), CancellationToken.None);

        result.Should().BeEquivalentTo(cityDto);
        _unitOfWorkMock.Verify(u => u.CityRepository.FirstOrDefaultAsync(cityId, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(cacheKey, cityDto, null, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenCityDoesNotExist()
    {
        var cityId = 999;
        var cacheKey = $"city:{cityId}";

        _cacheServiceMock.Setup(c => c.GetAsync<CityResponseDto>(cacheKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CityResponseDto);
        _unitOfWorkMock.Setup(u => u.CityRepository.FirstOrDefaultAsync(cityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as City);

        Func<Task> act = async () => await _handler.Handle(new GetCityByIdQuery(cityId), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _unitOfWorkMock.Verify(u => u.CityRepository.FirstOrDefaultAsync(cityId, It.IsAny<CancellationToken>()), Times.Once);
        _cacheServiceMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<CityResponseDto>(), null, It.IsAny<CancellationToken>()), Times.Never);
    }
}
