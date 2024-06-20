using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Delete;

public sealed record DeleteProfileCommand(string ProfileId) : IRequest<GeneralResponseDto>
{
    
}