using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetRecsByProfileId;

public sealed record GetRecsByProfileIdQuery(string ProfileId) : IRequest<IEnumerable<ProfileResponseDto>>
{
    
}