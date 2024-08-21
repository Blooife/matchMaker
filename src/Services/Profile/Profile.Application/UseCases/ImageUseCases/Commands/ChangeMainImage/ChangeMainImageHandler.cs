using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ImageUseCases.Commands.ChangeMainImage;

public class ChangeMainImageHandler(IUnitOfWork _unitOfWork, ICacheService _cacheService, IMapper _mapper, IProducerService _producerService): IRequestHandler<ChangeMainImageCommand, IEnumerable<ImageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";

    public async Task<IEnumerable<ImageResponseDto>> Handle(ChangeMainImageCommand request, CancellationToken cancellationToken)
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
        
        var notMainImage = profile.Images.First(p => p.IsMainImage);
        var mainImage = profile.Images.First(p => p.Id == image.Id);
        notMainImage.IsMainImage = false;
        mainImage.IsMainImage = true;
        
        await _unitOfWork.ImageRepository.UpdateImageAsync(notMainImage);
        
        await _unitOfWork.ImageRepository.UpdateImageAsync(mainImage);
        
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var sortedImages = profile.Images
            .OrderByDescending(i => i.IsMainImage)
            .ThenByDescending(i => i.UploadTimestamp)
            .ToList();
        profile.Images = sortedImages;
        
        var message = _mapper.Map<ProfileUpdatedMessage>(profile);
        
        await _producerService.ProduceAsync(message);
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return  _mapper.Map<IEnumerable<ImageResponseDto>>(profile.Images);
    }
}