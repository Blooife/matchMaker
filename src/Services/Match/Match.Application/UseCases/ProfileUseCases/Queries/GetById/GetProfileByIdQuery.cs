using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetById;

public sealed record GetProfileByIdQuery(string ProfileId) : IRequest<ProfileResponseDto>
{
    
}