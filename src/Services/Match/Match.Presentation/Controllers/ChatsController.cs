using Match.Application.DTOs.Chat.Request;
using Match.Application.UseCases.ChatUseCases.Commands.Create;
using Match.Application.UseCases.ChatUseCases.Commands.Delete;
using Match.Application.UseCases.ChatUseCases.Queries.GetByProfilesIds;
using Match.Application.UseCases.ChatUseCases.Queries.GetPaged;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Constants;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
public class ChatsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("profiles")]
    public async Task<IActionResult> GetByProfilesIds(CancellationToken cancellationToken, [FromQuery] string firstProfileId, [FromQuery] string secondProfileId)
    {
        var query = new GetChatByProfilesIdsQuery(firstProfileId, secondProfileId);

        var chat = await _mediator.Send(query, cancellationToken);

        return Ok(chat);
    }
    
    [HttpGet("paged/profiles/{profileId}")]
    public async Task<IActionResult> GetPagedChats([FromRoute] string profileId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetPagedChatsQuery(profileId, pageNumber, pageSize);

        var pagedList = await _mediator.Send(query, cancellationToken);
        var metadata = new
        {
            pagedList.TotalCount,
            pagedList.PageSize,
            pagedList.CurrentPage,
            pagedList.TotalPages,
            pagedList.HasNext,
            pagedList.HasPrevious
        };
        
        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return Ok(pagedList);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateChatCommand(dto);

        var chat = await _mediator.Send(command, cancellationToken);

        return Ok(chat);
    }
    
    [HttpDelete("{chatId}")]
    public async Task<IActionResult> DeleteChat([FromRoute] string chatId, CancellationToken cancellationToken)
    {
        var command = new DeleteChatCommand(chatId);

        var chat = await _mediator.Send(command, cancellationToken);

        return Ok(chat);
    }
}