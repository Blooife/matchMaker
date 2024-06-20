using Match.Application.DTOs.Profile.Request;
using Match.Application.DTOs.Profile.Response;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Update;

public sealed record UpdateProfileCommand(UpdateProfileDto Dto) : IRequest<ProfileResponseDto>
{
    
}