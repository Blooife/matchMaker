using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService, ProducerService _producerService) : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
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
        
        var fullProfile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == profile.Id, cancellationToken);
        var mappedProfile = _mapper.Map<ProfileResponseDto>(fullProfile);
        
        var cacheKey = $"{_cacheKeyPrefix}:{result.Id}";
        await _cacheService.SetAsync(cacheKey, mappedProfile, cancellationToken:cancellationToken);
        
        var message = _mapper.Map<ProfileUpdatedMessage>(fullProfile);
        await _producerService.ProduceAsync(message);
        
        return mappedProfile;
    }
}