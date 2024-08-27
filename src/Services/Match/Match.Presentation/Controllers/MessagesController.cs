using AutoMapper;
using Match.Application.UseCases.MessageUseCases.Queries.GetPaged;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Models;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.User}")]
public class MessagesController(IMediator _mediator, IMapper _mapper) : ControllerBase
{
    [HttpGet("paged/{chatId}")]
    public async Task<IActionResult> GetPagedMessages([FromRoute] string chatId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetPagedMessagesQuery(chatId, pageNumber, pageSize);

        var pagedList = await _mediator.Send(query, cancellationToken);
        var metadata = _mapper.Map<PaginationMetadata>(pagedList);
        
        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return Ok(pagedList);
    }
}