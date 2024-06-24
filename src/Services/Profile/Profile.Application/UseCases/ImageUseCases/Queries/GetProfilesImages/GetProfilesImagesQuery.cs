using MediatR;
using Profile.Application.DTOs.Image.Response;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetProfilesImages;

public sealed record GetProfilesImagesQuery(string ProfileId) : IRequest<IEnumerable<ImageResponseDto>>
{
    
}