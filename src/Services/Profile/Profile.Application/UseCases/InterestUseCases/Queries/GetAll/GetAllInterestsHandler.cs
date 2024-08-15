using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetAll;

public class GetAllInterestsHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllInterestsQuery, IEnumerable<InterestResponseDto>>
{
    private readonly string _cacheKeyPrefix = "interests";
    
    public async Task<IEnumerable<InterestResponseDto>> Handle(GetAllInterestsQuery request, CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<InterestResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var interests = await _unitOfWork.InterestRepository.GetAllAsync(cancellationToken);
        
        var mappedInterests = _mapper.Map<List<InterestResponseDto>>(interests);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedInterests, cancellationToken:cancellationToken);
        
        return mappedInterests;
    }
}