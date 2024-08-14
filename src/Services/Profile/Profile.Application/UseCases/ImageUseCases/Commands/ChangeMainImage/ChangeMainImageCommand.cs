using MediatR;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;

namespace Profile.Application.UseCases.ImageUseCases.Commands.ChangeMainImage;

public sealed record ChangeMainImageCommand(ChangeMainImageDto Dto): IRequest<IEnumerable<ImageResponseDto>>
{
    
}