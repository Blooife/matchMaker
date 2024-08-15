using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Application.Services;
using Match.Domain.Interfaces.Repositories;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetRecs;

public class GetRecsHandler(IUnitOfWork _unitOfWork, IProfileGrpcClient _client) : IRequestHandler<GetRecsQuery, List<ProfileResponseDto>>
{
    public async Task<List<ProfileResponseDto>> Handle(GetRecsQuery request, CancellationToken cancellationToken)
    {
        var userProfile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);

        if (userProfile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var likedProfiles = await _unitOfWork.Likes
            .GetAsync(like => like.ProfileId == request.ProfileId, cancellationToken);
        var likedProfilesIds = likedProfiles.Select(l => l.TargetProfileId);
        
        var matchedProfiles = await _unitOfWork.Matches
            .GetAsync(match => match.FirstProfileId == request.ProfileId || match.SecondProfileId == request.ProfileId, cancellationToken);
        var matchedProfilesIds = matchedProfiles.Select(match => match.FirstProfileId == request.ProfileId ? match.SecondProfileId : match.FirstProfileId);

        var excludedProfileIds = likedProfilesIds.Concat(matchedProfilesIds).Distinct().ToList();

        var ids =
            await _unitOfWork.Profiles.GetRecsAsync(excludedProfileIds, userProfile, cancellationToken);
        
        var profiles = await _client.GetProfilesInfo(ids);
        
        return profiles;
    }
}