using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetProfilesInterests;

public class GetProfilesInterestsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfilesInterestsQuery, IEnumerable<InterestResponseDto>>
{
    public async Task<IEnumerable<InterestResponseDto>> Handle(GetProfilesInterestsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var interests = await _unitOfWork.InterestRepository.GetProfilesInterests(request.ProfileId, cancellationToken);
        
        return _mapper.Map<IEnumerable<InterestResponseDto>>(interests);
    }
}