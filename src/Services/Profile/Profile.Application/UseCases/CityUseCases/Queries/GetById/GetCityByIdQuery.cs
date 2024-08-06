using MediatR;
using Profile.Application.DTOs.City.Response;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetById;

public sealed record GetCityByIdQuery(int CityId) : IRequest<CityResponseDto>;