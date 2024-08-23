using Bogus;
using Profile.Application.DTOs.Country.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class CountryFakers
{
    public static Faker<Country> CreateCountry()
    {
        return new Faker<Country>()
            .RuleFor(c => c.Id, f => f.Random.Int())
            .RuleFor(c => c.Name, f => f.Address.Country());
    }

    public static Faker<CountryResponseDto> CreateCountryResponseDto()
    {
        return new Faker<CountryResponseDto>()
            .RuleFor(c => c.Id, f => f.Random.Int())
            .RuleFor(c => c.Name, f => f.Address.Country());
    }
}