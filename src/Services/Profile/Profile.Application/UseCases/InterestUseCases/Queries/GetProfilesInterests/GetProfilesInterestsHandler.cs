using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Interest.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetProfilesInterests;

public class GetProfilesInterestsHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetProfilesInterestsQuery, IEnumerable<InterestResponseDto>>
{
    private readonly string _cacheKeyPrefix = "interest";
    
    public async Task<IEnumerable<InterestResponseDto>> Handle(GetProfilesInterestsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.ProfileId}";
        var cachedData = await _cacheService.GetAsync<List<InterestResponseDto>>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var interests = await _unitOfWork.InterestRepository.GetProfilesInterestsAsync(request.ProfileId, cancellationToken);
        var mappedInterests = _mapper.Map<List<InterestResponseDto>>(interests);
        await _cacheService.SetAsync(cacheKey, mappedInterests, cancellationToken: cancellationToken);
        
        return mappedInterests;
    }
}