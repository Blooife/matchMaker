using Match.Application.UseCases.ChatUseCases.Queries.GetByProfileId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetByProfileId([FromRoute]string id, CancellationToken cancellationToken)
    {
        var query = new GetChatsByProfileIdQuery(id);

        var chats = await _mediator.Send(query, cancellationToken);

        return Ok(chats);
    }
}