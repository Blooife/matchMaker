using AutoMapper;
using Match.Application.DTOs.Match.Response;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetByProfileId;

public class GetMatchesByProfileIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetMatchesByProfileIdQuery, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetMatchesByProfileIdQuery request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile == null)
        {
            
        }
        
        var matchRepository = _unitOfWork.Matches;
        var matches = await matchRepository.GetMatchesByProfileIdAsync(request.ProfileId, cancellationToken);
        var matchProfiles = matches
            .Select(m => 
            
                m.ProfileId1 == request.ProfileId ? m.ProfileId2 : m.ProfileId1
            )
            .ToList();
        
        return matchProfiles;
    }
}