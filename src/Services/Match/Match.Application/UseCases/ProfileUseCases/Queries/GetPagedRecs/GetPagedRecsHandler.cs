using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Application.Services.Interfaces;
using Match.Domain.Interfaces;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Queries.GetPagedRecs;

public class GetPagedRecsHandler(IUnitOfWork _unitOfWork, IProfileGrpcClient _client) : IRequestHandler<GetPagedRecsQuery, PagedList<FullProfileResponseDto>>
{
    public async Task<PagedList<FullProfileResponseDto>> Handle(GetPagedRecsQuery request, CancellationToken cancellationToken)
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

        var pagedRecs =
            await _unitOfWork.Profiles.GetPagedRecsAsync(excludedProfileIds, userProfile, request.PageNumber, request.PageSize, cancellationToken);
        
        var profiles = await _client.GetProfilesInfo(pagedRecs.Ids);
        
        return new PagedList<FullProfileResponseDto>(profiles, pagedRecs.TotalCount, request.PageNumber, request.PageSize);
    }
}