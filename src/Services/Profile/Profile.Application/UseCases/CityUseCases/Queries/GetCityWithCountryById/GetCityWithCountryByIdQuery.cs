using MediatR;
using Profile.Application.DTOs.City.Response;

namespace Profile.Application.UseCases.CityUseCases.Queries.GetCityWithCountryById;

public sealed record GetCityWithCountryByIdQuery(int CityId) : IRequest<CityWithCountryResponseDto>;