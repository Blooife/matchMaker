using Match.Application.UseCases.MessageUseCases.Queries.GetPaged;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(IMediator _mediator) : ControllerBase
{
    [HttpGet("paged/{chatId}")]
    public async Task<IActionResult> GetPagedMessages([FromRoute] string chatId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetPagedMessagesQuery(chatId, pageNumber, pageSize);

        var messages = await _mediator.Send(query, cancellationToken);

        return Ok(messages);
    }
}