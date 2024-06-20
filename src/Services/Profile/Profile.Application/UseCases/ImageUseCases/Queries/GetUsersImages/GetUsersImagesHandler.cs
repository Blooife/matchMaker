using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetUsersImages;

public class GetUsersImagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUsersImagesQuery, IEnumerable<ImageResponseDto>>
{
    public async Task<IEnumerable<ImageResponseDto>> Handle(GetUsersImagesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var images = await _unitOfWork.ImageRepository.GetUsersImages(request.ProfileId, cancellationToken);

        return _mapper.Map<IEnumerable<ImageResponseDto>>(images);
    }
}