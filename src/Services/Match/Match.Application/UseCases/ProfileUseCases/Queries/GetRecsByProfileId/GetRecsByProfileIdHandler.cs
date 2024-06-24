using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetRecsByProfileId;

public class GetRecsByProfileIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetRecsByProfileIdQuery, IEnumerable<ProfileResponseDto>>
{
    public async Task<IEnumerable<ProfileResponseDto>> Handle(GetRecsByProfileIdQuery request, CancellationToken cancellationToken)
    {
        string profileId = request.ProfileId;
        var userProfile = await _unitOfWork.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (userProfile is null)
        {
            throw new NotFoundException();
        }

        var likedProfiles = await _unitOfWork.Likes
                                        .GetAsync(like => like.ProfileId == profileId, cancellationToken);
        var likedProfilesIds = likedProfiles.Select(l => l.TargetProfileId);
        
        var matchedProfiles = await _unitOfWork.Matches
                                          .GetAsync(match => match.ProfileId1 == profileId || match.ProfileId2 == profileId, cancellationToken);
        var matchedProfilesIds = matchedProfiles.Select(match => match.ProfileId1 == profileId ? match.ProfileId2 : match.ProfileId1);

        var excludedProfileIds = likedProfilesIds.Concat(matchedProfilesIds).Distinct().ToList();

        var recommendations =
        await _unitOfWork.Profiles.GetRecommendationsAsync(excludedProfileIds, userProfile, true, cancellationToken);

        return _mapper.Map<IEnumerable<ProfileResponseDto>>(recommendations);
    }
}