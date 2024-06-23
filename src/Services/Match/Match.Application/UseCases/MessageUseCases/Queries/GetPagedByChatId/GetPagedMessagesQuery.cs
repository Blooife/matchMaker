using Match.Application.DTOs.Message.Request;
using Match.Application.DTOs.Message.Response;
using Match.Domain.Models;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MessageUseCases.Queries.GetPagedByChatId;

public sealed record GetPagedMessagesQuery(PagedMessagesDto Dto) : IRequest<PagedList<Message>>
{
    
}