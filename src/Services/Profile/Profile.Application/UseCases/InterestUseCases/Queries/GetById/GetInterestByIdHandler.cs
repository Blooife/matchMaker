using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetById;

public class GetInterestByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetInterestByIdQuery, InterestResponseDto>
{
    private readonly string _cacheKeyPrefix = "interest";
    
    public async Task<InterestResponseDto> Handle(GetInterestByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.InterestId}";
        var cachedData = await _cacheService.GetAsync<InterestResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var interest = await _unitOfWork.InterestRepository.FirstOrDefaultAsync(request.InterestId, cancellationToken);
        
        if (interest == null)
        {
            throw new NotFoundException("Interest", request.InterestId);
        }
        
        var mappedInterest = _mapper.Map<InterestResponseDto>(interest);
        await _cacheService.SetAsync(cacheKey, mappedInterest, cancellationToken:cancellationToken);
        
        return mappedInterest;
    }
}