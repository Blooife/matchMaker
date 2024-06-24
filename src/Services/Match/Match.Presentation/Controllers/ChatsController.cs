using Match.Application.DTOs.Chat.Request;
using Match.Application.UseCases.ChatUseCases.Commands.Create;
using Match.Application.UseCases.ChatUseCases.Queries.GetByProfileId;
using Match.Application.UseCases.ChatUseCases.Queries.GetPaged;
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
    
    [HttpGet("paged/{profileId}")]
    public async Task<IActionResult> GetPagedChats([FromRoute] string profileId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetPagedChatsQuery(profileId, pageNumber, pageSize);

        var chats = await _mediator.Send(query, cancellationToken);

        return Ok(chats);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateChatCommand(dto);

        var chat = await _mediator.Send(command, cancellationToken);

        return Ok(chat);
    }
}