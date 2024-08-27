using Match.Application.DTOs.Chat.Response;
using MediatR;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetByProfilesIds;

public sealed record GetChatByProfilesIdsQuery(string FirstProfileId, string SecondProfileId) : IRequest<ChatResponseDto>
{
    
}