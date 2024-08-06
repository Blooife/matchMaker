using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetById;

public sealed record GetProfileByIdQuery(string ProfileId) : IRequest<ProfileResponseDto>
{
    
}