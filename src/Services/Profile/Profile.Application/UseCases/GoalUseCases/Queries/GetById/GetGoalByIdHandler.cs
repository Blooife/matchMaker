using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Goal.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetById;

public class GetGoalByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetGoalByIdQuery, GoalResponseDto>
{
    public async Task<GoalResponseDto> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var goal = _unitOfWork.GoalRepository.FirstOrDefaultAsync(request.GoalId, cancellationToken);

        return _mapper.Map<GoalResponseDto>(goal);
    }
}