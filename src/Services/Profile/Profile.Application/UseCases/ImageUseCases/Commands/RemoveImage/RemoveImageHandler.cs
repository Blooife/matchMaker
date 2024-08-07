using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;

public class RemoveImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMinioService _minioService, ICacheService _cacheService, ProducerService _producerService) : IRequestHandler<RemoveImageCommand, ImageResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<ImageResponseDto> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.Dto.ProfileId}";
        var profileResponseDto = await _cacheService.GetAsync(cacheKey, async () =>
        {
            var profile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == request.Dto.ProfileId, cancellationToken);
            
            return _mapper.Map<ProfileResponseDto>(profile);
        }, cancellationToken);

        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(request.Dto.ImageId, cancellationToken);
        
        if (image is null)
        {
            throw new NotFoundException("Image", request.Dto.ImageId);
        }
        
        await _unitOfWork.ImageRepository.RemoveImageFromProfileAsync(image, cancellationToken);
        profile.Images.Remove(image);
        
        if (!profile.Images[0].IsMainImage)
        {
            profile.Images[0].IsMainImage = true;
            await _unitOfWork.ImageRepository.UpdateImageAsync(profile.Images[0], cancellationToken);
            
            var message = _mapper.Map<ProfileUpdatedMessage>(profile);
            await _producerService.ProduceAsync(message);
        }
        
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _minioService.DeleteFileAsync(image.ImageUrl);
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return  _mapper.Map<ImageResponseDto>(image);
    }
}