using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetAll;

public class GetAllInterestsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllInterestsQuery, IEnumerable<InterestResponseDto>>
{
    public async Task<IEnumerable<InterestResponseDto>> Handle(GetAllInterestsQuery request, CancellationToken cancellationToken)
    {
        var interests = await _unitOfWork.InterestRepository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<InterestResponseDto>>(interests);
    }
}