using Match.Application.DTOs.Profile.Request;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Update;

public sealed record UpdateProfileCommand(UpdateProfileDto Dto) : IRequest
{
    
}