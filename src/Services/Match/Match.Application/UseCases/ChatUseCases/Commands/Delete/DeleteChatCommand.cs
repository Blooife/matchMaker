using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ChatUseCases.Commands.Delete;

public sealed record DeleteChatCommand(int ChatId) : IRequest<GeneralResponseDto>
{
    
}