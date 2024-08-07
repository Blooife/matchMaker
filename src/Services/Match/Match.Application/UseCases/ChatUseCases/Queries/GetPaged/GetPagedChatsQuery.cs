using Match.Application.DTOs.Chat.Response;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ChatUseCases.Queries.GetPaged;

public sealed record GetPagedChatsQuery(string ProfileId, int PageNumber, int PageSize) : IRequest<PagedList<ChatResponseDto>>
{
    
}