using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Models;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ImageUseCases.Commands.AddImage;

public class AddImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMinioService _minioService, ICacheService _cacheService, IProducerService _producerService) : IRequestHandler<AddImageCommand, IEnumerable<ImageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
    
    public async Task<IEnumerable<ImageResponseDto>> Handle(AddImageCommand request, CancellationToken cancellationToken)
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
        
        var file = request.Dto.file;
        var fileExtension = Path.GetExtension(file.FileName); 

        if (!_allowedExtensions.Contains(fileExtension.ToLowerInvariant()))
        {
            throw new ImageUploadException("Wrong extension");
        }

        var objectName = $"{request.Dto.ProfileId}/{file.FileName}";
        
        await using var stream = new MemoryStream();
        
        await file.CopyToAsync(stream, cancellationToken);
        
        stream.Position = 0;
        
        await _minioService.UploadFileAsync(objectName, file);

        bool isMain = profile.Images.Count == 0;
        
        var imageEntity = new Image
        {
            ProfileId = request.Dto.ProfileId,
            ImageUrl = $"http://{_minioService.Endpoint}/{_minioService.BucketName}/{objectName}",
            IsMainImage = isMain,
            UploadTimestamp = DateTime.UtcNow
        };

        var result = await _unitOfWork.ImageRepository.AddImageToProfileAsync(imageEntity, cancellationToken);
        
        await _unitOfWork.SaveAsync(cancellationToken);
        
        profile.Images.Add(result);
        var sortedImages = profile.Images
            .OrderByDescending(i => i.IsMainImage)
            .ThenByDescending(i => i.UploadTimestamp)
            .ToList();
        profile.Images = sortedImages;
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);

        if (isMain)
        {
            var message = _mapper.Map<ProfileUpdatedMessage>(profile);
            
            await _producerService.ProduceAsync(message);
        }
        
        return _mapper.Map<IEnumerable<ImageResponseDto>>(sortedImages);
    }
}