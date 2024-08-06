using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetAll;

public sealed record GetAllProfilesQuery : IRequest<IEnumerable<ProfileResponseDto>>
{
    
}