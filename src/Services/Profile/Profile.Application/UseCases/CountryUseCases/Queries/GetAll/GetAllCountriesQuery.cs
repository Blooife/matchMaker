using MediatR;
using Profile.Application.DTOs.City.Response;
using Profile.Application.DTOs.Country.Response;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetAll;

public sealed record GetAllCountriesQuery : IRequest<IEnumerable<CountryResponseDto>>
{
    
}