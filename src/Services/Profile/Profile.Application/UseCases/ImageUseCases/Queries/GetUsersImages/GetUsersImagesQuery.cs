using MediatR;
using Profile.Application.DTOs.Image.Response;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetUsersImages;

public sealed record GetUsersImagesQuery(string ProfileId) : IRequest<IEnumerable<ImageResponseDto>>
{
    
}