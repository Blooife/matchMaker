using AutoMapper;
using Match.Application.UseCases.MatchUseCases.Queries.GetPaged;
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
public class MatchesController(IMediator _mediator, IMapper _mapper) : ControllerBase
{
    [HttpGet("paged/profiles/{profileId}")]
    public async Task<IActionResult> GetPagedMatches([FromRoute] string profileId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetPagedMatchesQuery(profileId, pageNumber, pageSize);

        var pagedList = await _mediator.Send(query, cancellationToken);
        var metadata = _mapper.Map<PaginationMetadata>(pagedList);

        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        return Ok(pagedList);
    }
}