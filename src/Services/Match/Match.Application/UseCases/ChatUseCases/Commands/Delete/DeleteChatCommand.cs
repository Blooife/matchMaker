using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ChatUseCases.Commands.Delete;

public sealed record DeleteChatCommand(string ChatId) : IRequest<GeneralResponseDto>
{
    
}