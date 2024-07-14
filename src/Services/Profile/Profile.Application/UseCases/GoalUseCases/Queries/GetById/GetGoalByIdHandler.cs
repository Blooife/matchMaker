using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Goal.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetById;

public class GetGoalByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetGoalByIdQuery, GoalResponseDto>
{
    public async Task<GoalResponseDto> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var goal = await _unitOfWork.GoalRepository.FirstOrDefaultAsync(request.GoalId, cancellationToken);

        if (goal is null)
        {
            throw new NotFoundException("Goal", request.GoalId);    
        }
        
        return _mapper.Map<GoalResponseDto>(goal);
    }
}