using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Goal.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetById;

public class GetGoalByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetGoalByIdQuery, GoalResponseDto>
{
    private readonly string _cacheKeyPrefix = "goal";
    
    public async Task<GoalResponseDto> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.GoalId}";
        var cachedData = await _cacheService.GetAsync<GoalResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var goal = await _unitOfWork.GoalRepository.FirstOrDefaultAsync(request.GoalId, cancellationToken);

        if (goal is null)
        {
            throw new NotFoundException("Goal", request.GoalId);    
        }
        
        var mappedGoal = _mapper.Map<GoalResponseDto>(goal);
        await _cacheService.SetAsync(cacheKey, mappedGoal, cancellationToken:cancellationToken);
        
        return mappedGoal;
    }
}