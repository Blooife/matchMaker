using MediatR;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.DTOs.Image.Response;

namespace Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;

public sealed record RemoveImageCommand(RemoveImageDto Dto) : IRequest<ImageResponseDto>
{
    
}