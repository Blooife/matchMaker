using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Services.Interfaces;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService, ProducerService _producerService) : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = _mapper.Map<UserProfile>(request.CreateProfileDto);
        var result = await _unitOfWork.ProfileRepository.CreateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var fullProfile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == profile.Id, cancellationToken);
        var mappedProfile = _mapper.Map<ProfileResponseDto>(fullProfile);
        
        var cacheKey = $"{_cacheKeyPrefix}:{result.Id}";
        await _cacheService.SetAsync(cacheKey, mappedProfile, cancellationToken:cancellationToken);
        
        var message = _mapper.Map<ProfileCreatedMessage>(fullProfile);
        await _producerService.ProduceAsync(message);
        
        return mappedProfile;
    }
}