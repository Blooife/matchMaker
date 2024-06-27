using Match.Application.DTOs.Like.Request;
using Match.Application.DTOs.Like.Response;
using MediatR;

namespace Match.Application.UseCases.LikeUseCases.Commands.Add;

public sealed record AddLikeCommand(AddLikeDto Dto) : IRequest<LikeResponseDto>
{
    
}