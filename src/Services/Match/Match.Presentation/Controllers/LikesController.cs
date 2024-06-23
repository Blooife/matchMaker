using Match.Application.DTOs.Like.Request;
using Match.Application.UseCases.LikeUseCases.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateLike([FromBody]CreateLikeDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateLikeCommand(dto);

        var recs = await _mediator.Send(command, cancellationToken);

        return Ok(recs);
    }
}