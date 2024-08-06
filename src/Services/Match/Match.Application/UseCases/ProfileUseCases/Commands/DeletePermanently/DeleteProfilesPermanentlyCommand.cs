using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;

public sealed record DeleteProfilesPermanentlyCommand(List<string> Ids) : IRequest
{
    
}