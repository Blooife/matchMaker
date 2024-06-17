using MediatR;
using Profile.Application.DTOs.City.Response;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetAll;

public sealed record GetAllCitiesQuery : IRequest<IEnumerable<CityResponseDto>>
{
    
}