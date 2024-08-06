using Match.Application.Exceptions;
using Match.Domain.Models;
using Match.Domain.Interfaces;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetPaged;

public class GetPagedMatchesHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetPagedMatchesQuery, PagedList<MatchEntity>>
{
    public async Task<PagedList<MatchEntity>> Handle(GetPagedMatchesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var matches = await _unitOfWork.Matches.GetPagedAsync(request.ProfileId, request.PageNumber, request.PageSize, cancellationToken);

        return matches;
    }
}