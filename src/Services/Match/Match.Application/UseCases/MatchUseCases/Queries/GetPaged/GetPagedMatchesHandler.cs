using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Application.Services.Interfaces;
using Match.Domain.Interfaces;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetPaged;

public class GetPagedMatchesHandler(IUnitOfWork _unitOfWork, IProfileGrpcClient _client) : IRequestHandler<GetPagedMatchesQuery, PagedList<ProfileResponseDto>>
{
    public async Task<PagedList<ProfileResponseDto>> Handle(GetPagedMatchesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);

        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }

        var result = await _unitOfWork.Matches.GetPagedAsync(request.ProfileId, request.PageNumber, request.PageSize, cancellationToken);
        var filteredIds = result.Item1
            .SelectMany(m => new[] { m.FirstProfileId, m.SecondProfileId })  
            .Where(id => id != request.ProfileId)  
            .Distinct()  
            .ToList();
        var profiles = await _client.GetProfilesInfo(filteredIds);
        
        return new PagedList<ProfileResponseDto>(profiles, result.Item2, request.PageNumber, request.PageSize);
    }
}