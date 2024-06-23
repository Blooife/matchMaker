using MediatR;
using Profile.Application.DTOs.Image.Response;

namespace Profile.Application.UseCases.ImageUseCases.Queries.GetById;

public sealed record GetImageByIdQuery(int ImageId) : IRequest<ImageResponseDto>
{
    
}