using Match.Application.DTOs.Like.Request;
using Match.Application.DTOs.Like.Response;
using MediatR;

namespace Match.Application.UseCases.LikeUseCases.Commands.Create;

public sealed record CreateLikeCommand(CreateLikeDto Dto) : IRequest<LikeResponseDto>
{
    
}