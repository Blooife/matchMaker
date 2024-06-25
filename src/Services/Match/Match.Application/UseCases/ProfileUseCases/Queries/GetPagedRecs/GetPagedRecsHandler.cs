using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetPagedRecs;

public class GetPagedRecsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPagedRecsQuery, PagedList<Profile>>
{
    public async Task<PagedList<Profile>> Handle(GetPagedRecsQuery request, CancellationToken cancellationToken)
    {
        string profileId = request.ProfileId;
        var userProfile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (userProfile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var likedProfiles = await _unitOfWork.Likes
            .GetAsync(like => like.ProfileId == profileId, cancellationToken);
        var likedProfilesIds = likedProfiles.Select(l => l.TargetProfileId);
        
        var matchedProfiles = await _unitOfWork.Matches
            .GetAsync(match => match.ProfileId1 == profileId || match.ProfileId2 == profileId, cancellationToken);
        var matchedProfilesIds = matchedProfiles.Select(match => match.ProfileId1 == profileId ? match.ProfileId2 : match.ProfileId1);

        var excludedProfileIds = likedProfilesIds.Concat(matchedProfilesIds).Distinct().ToList();

        var recommendations =
            await _unitOfWork.Profiles.GetPagedRecsAsync(excludedProfileIds, userProfile, request.PageNumber, request.PageSize, cancellationToken);

        return recommendations;
    }
}