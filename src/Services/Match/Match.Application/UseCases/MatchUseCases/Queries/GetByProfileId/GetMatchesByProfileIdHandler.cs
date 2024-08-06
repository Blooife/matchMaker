using AutoMapper;
using Match.Application.DTOs.Match.Response;
using Match.Application.Exceptions;
using Match.Domain.Interfaces;
using MediatR;

namespace Match.Application.UseCases.MatchUseCases.Queries.GetByProfileId;

public class GetMatchesByProfileIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetMatchesByProfileIdQuery, IEnumerable<MatchResponseDto>>
{
    public async Task<IEnumerable<MatchResponseDto>> Handle(GetMatchesByProfileIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var matches = await _unitOfWork.Matches.GetMatchesByProfileIdAsync(request.ProfileId, cancellationToken);
        
        return _mapper.Map<IEnumerable<MatchResponseDto>>(matches);
    }
}