using Match.Application.DTOs.Chat.Response;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetByProfileId;

public sealed record GetChatsByProfileIdQuery(string ProfileId) : IRequest<IEnumerable<ChatResponseDto>>
{
    
}