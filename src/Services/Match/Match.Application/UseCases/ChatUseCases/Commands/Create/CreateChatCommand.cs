using Match.Application.DTOs.Chat.Request;
using Match.Application.DTOs.Chat.Response;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Commands.Create;

public sealed record CreateChatCommand(CreateChatDto Dto) : IRequest<ChatResponseDto>
{
    
}