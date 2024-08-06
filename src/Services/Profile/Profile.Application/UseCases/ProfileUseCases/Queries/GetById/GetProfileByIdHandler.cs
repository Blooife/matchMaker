using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;

using Profile.Application.Services.Interfaces;

using Profile.Domain.Interfaces;


namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetById;

public class GetProfileByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.ProfileId}";
        var cachedData = await _cacheService.GetAsync<ProfileResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var mappedProfile = _mapper.Map<ProfileResponseDto>(profile);
        await _cacheService.SetAsync(cacheKey, mappedProfile, cancellationToken:cancellationToken);
        
        return mappedProfile;
    }
}