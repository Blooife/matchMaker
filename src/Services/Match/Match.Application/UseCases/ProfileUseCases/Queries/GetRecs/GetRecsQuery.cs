using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetRecs;

public sealed record GetRecsQuery(string ProfileId) : IRequest<List<ProfileResponseDto>>;