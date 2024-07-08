using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = _mapper.Map<UserProfile>(request.CreateProfileDto);
        var result = await _unitOfWork.ProfileRepository.CreateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:{result.Id}";
        var mappedProfile = _mapper.Map<ProfileResponseDto>(result);
        await _cacheService.SetAsync(cacheKey, mappedProfile, cancellationToken:cancellationToken);
        
        return mappedProfile;
    }
}