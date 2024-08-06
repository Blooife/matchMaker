using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;

public class RemoveImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMinioService _minioService, ICacheService _cacheService) : IRequestHandler<RemoveImageCommand, ImageResponseDto>
{
    private readonly string _cacheKeyPrefix = "image";
    
    public async Task<ImageResponseDto> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(request.Dto.ImageId, cancellationToken);
        
        if (image is null)
        {
            throw new NotFoundException("Image", request.Dto.ImageId);
        }
        
        var imageEntity = _mapper.Map<Image>(image);
        await _unitOfWork.ImageRepository.RemoveImageFromProfileAsync(imageEntity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _minioService.DeleteFileAsync(image.ImageUrl);
        
        var cacheKey = $"{_cacheKeyPrefix}:{imageEntity.Id}";
        var mappedImage = _mapper.Map<ImageResponseDto>(imageEntity);
        await _cacheService.RemoveAsync(cacheKey, cancellationToken:cancellationToken);
        
        return mappedImage;
    }
}