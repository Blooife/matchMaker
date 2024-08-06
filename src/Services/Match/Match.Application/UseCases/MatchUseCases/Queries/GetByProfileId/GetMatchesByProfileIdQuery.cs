using Match.Application.DTOs.Match.Response;
using MediatR;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetByProfileId;

public sealed record GetMatchesByProfileIdQuery(string ProfileId) : IRequest<IEnumerable<MatchResponseDto>>
{
    
}