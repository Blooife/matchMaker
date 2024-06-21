using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;

public class RemoveImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<RemoveImageCommand, ImageResponseDto>
{
    public async Task<ImageResponseDto> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
    {
        var image = await _unitOfWork.ImageRepository.GetByIdAsync(request.Dto.ImageId, cancellationToken);
        
        if (image is null)
        {
            throw new NotFoundException("Image", request.Dto.ImageId);
        }
        
        var imageEntity = _mapper.Map<Image>(image);
        await _unitOfWork.ImageRepository.RemoveImageFromProfile(imageEntity);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName; //to do directory
        var path = Path.Combine(parentDirectory, "", image.ImageUrl);

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            throw new FileNotFoundException("The file does not exist.");
        }
        
        return _mapper.Map<ImageResponseDto>(imageEntity);
    }
}