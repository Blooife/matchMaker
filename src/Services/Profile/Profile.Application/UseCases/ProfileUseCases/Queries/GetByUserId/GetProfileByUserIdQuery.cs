using MediatR;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetByUserId;

public sealed record GetProfileByUserIdQuery(string UserId) : IRequest<ProfileResponseDto>
{
    
}