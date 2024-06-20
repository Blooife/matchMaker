using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Delete;

public sealed record DeleteProfileCommand(string ProfileId) //: IRequest<>
{
    
}