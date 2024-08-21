using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Goal.Response;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetAll;

public class GetAllGoalsHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllGoalsQuery, IEnumerable<GoalResponseDto>>
{
    private readonly string _cacheKeyPrefix = "goals";
    
    public async Task<IEnumerable<GoalResponseDto>> Handle(GetAllGoalsQuery request, CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<GoalResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var goals = await _unitOfWork.GoalRepository.GetAllAsync(cancellationToken);
        
        var mappedGoals = _mapper.Map<List<GoalResponseDto>>(goals);
        
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedGoals, cancellationToken:cancellationToken);

        return mappedGoals;
    }
}