using MediatR;
using Profile.Application.DTOs.Profile.Request;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public sealed record UpdateProfileCommand(UpdateProfileDto UpdateProfileDto) : IRequest<ProfileResponseDto>
{
    
}