using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetById;

public class GetImageByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetImageByIdQuery, ImageResponseDto>
{
    private readonly string _cacheKeyPrefix = "image";
    
    public async Task<ImageResponseDto> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.ImageId}";
        var cachedData = await _cacheService.GetAsync<ImageResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(request.ImageId, cancellationToken);

        if (image is null)
        {
            throw new NotFoundException("Image", request.ImageId);
        }
        
        var mappedImage = _mapper.Map<ImageResponseDto>(image);
        await _cacheService.SetAsync(cacheKey, mappedImage, cancellationToken:cancellationToken);
        
        return mappedImage;
    }
}