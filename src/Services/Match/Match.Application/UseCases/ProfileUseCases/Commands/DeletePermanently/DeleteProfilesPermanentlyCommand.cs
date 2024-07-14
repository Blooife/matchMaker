
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;

public sealed record DeleteProfilesPermanentlyCommand(IEnumerable<string> Ids) : IRequest
{
    
}