using MediatR;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;

namespace Profile.Application.UseCases.ImageUseCases.Commands.AddImage;

public sealed record AddImageCommand(AddImageDto Dto) : IRequest<IEnumerable<ImageResponseDto>>
{
    
}