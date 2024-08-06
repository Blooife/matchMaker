using MediatR;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.DeletePermanently;

public sealed record DeleteProfilesPermanentlyCommand(List<string> Ids) : IRequest
{
    
}