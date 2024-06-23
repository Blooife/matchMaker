using AutoMapper;
using Match.Application.DTOs.Match.Response;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetByProfileId;

public class GetMatchesByProfileIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetMatchesByProfileIdQuery, IEnumerable<MatchResponseDto>>
{
    public async Task<IEnumerable<MatchResponseDto>> Handle(GetMatchesByProfileIdQuery request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException();
        }
        
        var matchRepository = _unitOfWork.Matches;
        var matches = await matchRepository.GetMatchesByProfileIdAsync(request.ProfileId, cancellationToken);
        
        return _mapper.Map<IEnumerable<MatchResponseDto>>(matches);
    }
}