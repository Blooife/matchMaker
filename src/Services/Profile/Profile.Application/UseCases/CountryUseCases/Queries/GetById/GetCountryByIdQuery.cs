using MediatR;
using Profile.Application.DTOs.Country.Response;

namespace Profile.Application.UseCases.CountryUseCases.Queries.GetById;

public sealed record GetCountryByIdQuery(int CountryId) : IRequest<CountryResponseDto>;