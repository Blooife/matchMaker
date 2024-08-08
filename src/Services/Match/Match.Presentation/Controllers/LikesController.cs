using Match.Application.DTOs.Like.Request;
using Match.Application.UseCases.LikeUseCases.Commands.Add;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class LikesController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddLike([FromBody]AddLikeDto dto, CancellationToken cancellationToken)
    {
        var command = new AddLikeCommand(dto);

        var like = await _mediator.Send(command, cancellationToken);

        return Ok(like);
    }
}