using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Goal.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetAll;

public class GetAllGoalsHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllGoalsQuery, IEnumerable<GoalResponseDto>>
{
    public async Task<IEnumerable<GoalResponseDto>> Handle(GetAllGoalsQuery request, CancellationToken cancellationToken)
    {
        var goals = await _unitOfWork.GoalRepository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<GoalResponseDto>>(goals);
    }
}