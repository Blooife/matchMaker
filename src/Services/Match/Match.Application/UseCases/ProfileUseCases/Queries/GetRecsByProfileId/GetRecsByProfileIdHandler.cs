using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Domain.Repositories;
using MediatR;
using MongoDB.Driver;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetRecsByProfileId;

public class GetRecsByProfileIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetRecsByProfileIdQuery, IEnumerable<ProfileResponseDto>>
{
    public async Task<IEnumerable<ProfileResponseDto>> Handle(GetRecsByProfileIdQuery request, CancellationToken cancellationToken)
    {
        var _dbContext = _unitOfWork;
        string profileId = request.ProfileId;
        var userProfile = await _dbContext.Profiles.GetByIdAsync(profileId, cancellationToken);

        if (userProfile == null)
        {
            
        }

        var likedProfiles = await _dbContext.Likes
                                        .GetAsync(like => like.ProfileId == profileId, cancellationToken);

        var l = likedProfiles.Select(l => l.TargetProfileId);
        var matchedProfiles = await _dbContext.Matches
                                          .GetAsync(match => match.ProfileId1 == profileId || match.ProfileId2 == profileId, cancellationToken);
        var m = matchedProfiles.Select(match => match.ProfileId1 == profileId ? match.ProfileId2 : match.ProfileId1);

        var excludedProfileIds = l.Concat(m).Distinct().ToList();

        var recommendations =
        await _dbContext.Profiles.GetRecommendationsAsync(excludedProfileIds, userProfile, true, cancellationToken);

        return _mapper.Map<IEnumerable<ProfileResponseDto>>(recommendations);
    }
}