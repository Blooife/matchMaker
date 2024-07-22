using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var findRes =
            await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.UpdateProfileDto.Id, cancellationToken);
        
        if (findRes is null)
        {
            throw new NotFoundException("Profile", request.UpdateProfileDto.Id);
        }
        
        var profile = _mapper.Map<UserProfile>(request.UpdateProfileDto);
        var result = await _unitOfWork.ProfileRepository.UpdateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:{result.Id}";
        var mappedProfile = _mapper.Map<ProfileResponseDto>(result);
        await _cacheService.SetAsync(cacheKey, mappedProfile, cancellationToken:cancellationToken);
        
        return mappedProfile;
    }
}