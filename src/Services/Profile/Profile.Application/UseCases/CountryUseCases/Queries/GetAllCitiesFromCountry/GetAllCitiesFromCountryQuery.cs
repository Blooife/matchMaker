using MediatR;
using Profile.Application.DTOs.City.Response;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;

public sealed record GetAllCitiesFromCountryQuery(int CountryId) : IRequest<IEnumerable<CityResponseDto>>
{
    
}