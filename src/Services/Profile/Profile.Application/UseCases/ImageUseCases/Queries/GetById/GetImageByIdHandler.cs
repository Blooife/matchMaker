using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetById;

public class GetImageByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetImageByIdQuery, ImageResponseDto>
{
    public async Task<ImageResponseDto> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
    {
        var image = await _unitOfWork.ImageRepository.FirstOrDefaultAsync(request.ImageId, cancellationToken);

        if (image is null)
        {
            throw new NotFoundException("Image", request.ImageId);
        }
        
        return _mapper.Map<ImageResponseDto>(image);
    }
}