using Bogus;
using Profile.Application.DTOs.City.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class CityFakers
{
    public static Faker<City> CreateCity()
    {
        return new Faker<City>()
            .RuleFor(c => c.Id, f => f.Random.Int())
            .RuleFor(c => c.Name, f => f.Address.City());
    }

    public static Faker<CityResponseDto> CreateCityResponseDto()
    {
        return new Faker<CityResponseDto>()
            .RuleFor(c => c.Id, f => f.Random.Int())
            .RuleFor(c => c.Name, f => f.Address.City());
    }
    
    public static Faker<CityWithCountryResponseDto> CreateCityWithCountryResponseDto()
    {
        return new Faker<CityWithCountryResponseDto>()
            .RuleFor(c => c.Id, f => f.Random.Int())
            .RuleFor(c => c.Name, f => f.Address.City())
            .RuleFor(c => c.Country, f => CountryFakers.CreateCountryResponseDto().Generate());
    }
}