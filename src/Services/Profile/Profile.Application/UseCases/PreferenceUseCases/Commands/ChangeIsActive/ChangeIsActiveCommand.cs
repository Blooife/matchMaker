using MediatR;
using Shared.Models;

namespace Profile.Application.UseCases.PreferenceUseCases.Commands.ChangeIsActive;

public sealed record ChangeIsActiveCommand(string ProfileId) : IRequest<GeneralResponseDto>
{
    
}