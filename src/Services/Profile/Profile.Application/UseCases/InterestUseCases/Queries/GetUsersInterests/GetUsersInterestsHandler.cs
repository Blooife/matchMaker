using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetUsersInterests;

public class GetUsersInterestsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUsersInterestsQuery, IEnumerable<InterestResponseDto>>
{
    public async Task<IEnumerable<InterestResponseDto>> Handle(GetUsersInterestsQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetProfileByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
        {
            //
        }
        
        var interests = await _unitOfWork.InterestRepository.GetUsersInterests(profile, cancellationToken);
        return _mapper.Map<IEnumerable<InterestResponseDto>>(interests);
    }
}