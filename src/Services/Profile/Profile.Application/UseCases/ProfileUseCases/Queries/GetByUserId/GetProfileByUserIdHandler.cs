using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetByUserId;

public class GetProfileByUserIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetProfileByUserIdQuery, ProfileResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ProfileResponseDto> Handle(GetProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(p=>p.UserId == request.UserId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.UserId);
        }
        
        var mappedProfile = _mapper.Map<ProfileResponseDto>(profile);
        var cacheKey = $"{_cacheKeyPrefix}:{profile.Id}";
        await _cacheService.SetAsync(cacheKey, mappedProfile, cancellationToken:cancellationToken);
        
        return mappedProfile;
    }
}