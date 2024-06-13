using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetAll;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetAll;

public class GetAllInterestsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllInterestsQuery, IEnumerable<InterestResponseDto>>
{
    public async Task<IEnumerable<InterestResponseDto>> Handle(GetAllInterestsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.InterestRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<InterestResponseDto>>(result);
    }
}