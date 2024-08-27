using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Profile.Application.DTOs.City.Response;
using Profile.Application.DTOs.Country.Response;
using Profile.Tests.Fakers;
using Profile.Tests.IntegrationTests.Fixtures;
using Shared.Models;

namespace Profile.Tests.IntegrationTests.ControllersTests;

public class CountriesControllerTests(CustomWebApplicationFactory factory) : ClassFixture(factory)
{
    private const string BaseUrl = "/api/countries";
    
    [Fact]
    public async Task GetAllCities_NoCitiesInDb_ReturnsEmptyCollection()
    {
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<CountryResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllCities_ReturnsCities()
    {
        await AddEntitiesToDbAsync(CountryFakers.CreateCountry().Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<CountryResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetCountryById_CountryExists_ReturnsCountry()
    {
        var country = CountryFakers.CreateCountry().Generate();
        await AddEntitiesToDbAsync(country);
        
        var response = await Client.GetAsync($"{BaseUrl}/{country.Id}");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<CountryResponseDto>();
        responseDto.Should().NotBeNull();
        responseDto!.Id.Should().Be(country.Id);
    }
    
    [Fact]
    public async Task GetCountryById_CountryNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
    
    [Fact]
    public async Task GetAllCitiesFromCountry_NoCitiesInDb_ReturnsEmptyCollection()
    {
        var country = CountryFakers.CreateCountry().Generate();
        await AddEntitiesToDbAsync(country);
        
        var response = await Client.GetAsync($"{BaseUrl}/{country.Id}/cities");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<CityResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllCitiesFromCountry_ReturnsCities()
    {
        var country = CountryFakers.CreateCountry().Generate();
        await AddEntitiesToDbAsync(country);
        await AddEntitiesToDbAsync(CityFakers.CreateCity().Clone().RuleFor(c=>c.CountryId, country.Id).Generate(5));
        
        var response = await Client.GetAsync($"{BaseUrl}/{country.Id}/cities");
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadFromJsonAsync<List<CityResponseDto>>();
        responseDto.Should().NotBeNull();
        responseDto!.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetAllCitiesFromCountry_CountryNotExists_ReturnsNotFound()
    {
        int notExistingId = -1;
        
        var response = await Client.GetAsync($"{BaseUrl}/{notExistingId}/cities");
        
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseDto = await response.Content.ReadFromJsonAsync<ErrorDetails>();
        responseDto.Should().NotBeNull();
        responseDto!.ErrorType.Should().Be("NotFoundError");
    }
}