using MediatR;
using Profile.Application.DTOs.Profile.Request;
using Profile.Application.DTOs.Profile.Response;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public sealed record CreateProfileCommand(CreateProfileDto CreateProfileDto) : IRequest<ProfileResponseDto>
{
    
}