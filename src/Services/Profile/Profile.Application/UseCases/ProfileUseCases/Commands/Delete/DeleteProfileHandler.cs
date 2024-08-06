using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Delete;

public class DeleteProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ProducerService _producerService, ICacheService _cacheService) : IRequestHandler<DeleteProfileCommand, ProfileResponseDto>

{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ProfileResponseDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        await _unitOfWork.ProfileRepository.DeleteProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:{profile.Id}";
        var mappedProfile = _mapper.Map<ProfileResponseDto>(profile);
        await _cacheService.RemoveAsync(cacheKey, cancellationToken:cancellationToken);
        
        var message = _mapper.Map<ProfileDeletedMessage>(profile);
        await _producerService.ProduceAsync(message);
        
        return mappedProfile;

    }
}