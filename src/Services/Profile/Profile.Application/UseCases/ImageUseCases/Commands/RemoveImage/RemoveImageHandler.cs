using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;

public class RemoveImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper, IMinioService _minioService) : IRequestHandler<RemoveImageCommand, ImageResponseDto>
{
    public async Task<ImageResponseDto> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(request.Dto.ImageId, cancellationToken);
        
        if (image is null)
        {
            throw new NotFoundException("Image", request.Dto.ImageId);
        }
        
        var imageEntity = _mapper.Map<Image>(image);
        await _unitOfWork.ImageRepository.RemoveImageFromProfile(imageEntity);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _minioService.DeleteFileAsync(image.ImageUrl);
        
        return _mapper.Map<ImageResponseDto>(imageEntity);
    }
}