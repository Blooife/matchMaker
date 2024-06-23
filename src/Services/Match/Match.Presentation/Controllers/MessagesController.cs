using Match.Application.DTOs.Message.Request;
using Match.Application.UseCases.MessageUseCases.Queries.GetPagedByChatId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController(IMediator _mediator) : ControllerBase
{
    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedMessages([FromBody] PagedMessagesDto dto, CancellationToken cancellationToken)
    {
        var query = new GetPagedMessagesQuery(dto);

        var messages = await _mediator.Send(query, cancellationToken);

        return Ok(messages);
    }
}