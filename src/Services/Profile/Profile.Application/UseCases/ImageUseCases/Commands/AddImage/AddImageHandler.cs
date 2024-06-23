using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Implementations;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Commands.AddImage;

public class AddImageHandler(IUnitOfWork _unitOfWork, IMapper _mapper, MinioService _minioService) : IRequestHandler<AddImageCommand, ImageResponseDto>
{
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

        var result = await _unitOfWork.ImageRepository.AddImageToProfile(imageEntity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<ImageResponseDto>(result);
    }
    
    /*public async Task<ImageResponseDto> Handle(AddImageCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }

        var file = request.Dto.file;
        
        var fileExtension = Path.GetExtension(file.FileName);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", };

        if (allowedExtensions.Contains(fileExtension.ToLowerInvariant()))
        {
            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName; //to do directory
            var path = Path.Combine(parentDirectory,"", request.Dto.ProfileId+file.FileName);

            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);
        }
        else
        {
            throw new ImageUploadException("Wrong extension");
        }

        var imageEntity = new Image()
        {
            ProfileId = request.Dto.ProfileId,
            ImageUrl = $"{request.Dto.ProfileId}{file.FileName}"
        };
        var result = await _unitOfWork.ImageRepository.AddImageToProfile(imageEntity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<ImageResponseDto>(result);
    }*/
}