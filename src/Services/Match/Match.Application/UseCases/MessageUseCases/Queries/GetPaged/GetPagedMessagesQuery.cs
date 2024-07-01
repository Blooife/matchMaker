using Match.Domain.Models;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MessageUseCases.Queries.GetPaged;

public sealed record GetPagedMessagesQuery(string ChatId, int PageNumber, int PageSize) : IRequest<PagedList<Message>>
{
    
}