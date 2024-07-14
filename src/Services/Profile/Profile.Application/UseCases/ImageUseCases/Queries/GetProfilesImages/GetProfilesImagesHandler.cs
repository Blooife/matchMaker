using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Image.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetProfilesImages;

public class GetProfilesImagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfilesImagesQuery, IEnumerable<ImageResponseDto>>
{
    public async Task<IEnumerable<ImageResponseDto>> Handle(GetProfilesImagesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var images = await _unitOfWork.ImageRepository.GetProfilesImages(request.ProfileId, cancellationToken);

        return _mapper.Map<IEnumerable<ImageResponseDto>>(images);
    }
}