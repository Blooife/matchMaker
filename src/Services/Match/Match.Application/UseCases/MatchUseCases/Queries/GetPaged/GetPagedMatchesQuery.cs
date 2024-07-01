using Match.Domain.Models;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetPaged;

public sealed record GetPagedMatchesQuery(string ProfileId, int PageNumber, int PageSize) : IRequest<PagedList<MatchEntity>>
{
    
}