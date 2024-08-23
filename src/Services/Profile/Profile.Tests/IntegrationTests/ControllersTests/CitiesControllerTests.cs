using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Profile.Application.DTOs.City.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class CitiesControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/cities";
    
    [Fact]
    public async Task GetAllCities_NoCitiesInDb_ReturnsEmptyCollection()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<CityResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllCities_ReturnsCities()
    {
        var country = CountryFakers.CreateCountry().Generate();
        await AddEntitiesToDbAsync(country);
        await AddEntitiesToDbAsync(CityFakers.CreateCity().Clone().RuleFor(c=>c.CountryId, country.Id).Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<CityResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetCityById_CityExists_ReturnsCity()
    {
        var country = CountryFakers.CreateCountry().Generate();
        var city = CityFakers.CreateCity().Clone().RuleFor(c => c.CountryId, country.Id).Generate();
        await AddEntitiesToDbAsync(country);
        await AddEntitiesToDbAsync(city);
        
        var response = await Client.GetAsync($"{BaseUrl}/{city.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<CityResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(city.Id);
    }
    
    [Fact]
    public async Task GetCityById_CityNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
}