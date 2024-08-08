using Match.Application.DTOs.Profile.Request;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Create;

public sealed record CreateProfileCommand(CreateProfileDto Dto) : IRequest
{
    
}