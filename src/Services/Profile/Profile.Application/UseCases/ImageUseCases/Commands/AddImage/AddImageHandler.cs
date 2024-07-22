using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Commands.AddImage;

public class AddImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMinioService _minioService, ICacheService _cacheService) : IRequestHandler<AddImageCommand, ImageResponseDto>
{
    private readonly string _cacheKeyPrefix = "image";
    
    public async Task<ImageResponseDto> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }

        var file = request.Dto.file;
        var fileExtension = Path.GetExtension(file.FileName);
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

        if (!allowedExtensions.Contains(fileExtension.ToLowerInvariant()))
        {
            throw new ImageUploadException("Wrong extension");
        }

        var objectName = $"{request.Dto.ProfileId}/{file.FileName}";
        
        await using var stream = new MemoryStream();
        await file.CopyToAsync(stream, cancellationToken);
        
        stream.Position = 0;

        await _minioService.UploadFileAsync(objectName, file);

        var imageEntity = new Image
        {
            ProfileId = request.Dto.ProfileId,
            ImageUrl = objectName 
        };

        var result = await _unitOfWork.ImageRepository.AddImageToProfileAsync(imageEntity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:{result.Id}";
        var mappedImage = _mapper.Map<ImageResponseDto>(result);
        await _cacheService.SetAsync(cacheKey, mappedImage, cancellationToken:cancellationToken);
        
        return mappedImage;
    }
}