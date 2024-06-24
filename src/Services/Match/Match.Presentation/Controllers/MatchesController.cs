using Match.Application.UseCases.MatchUseCases.Queries.GetByProfileId;
using Match.Application.UseCases.MatchUseCases.Queries.GetPaged;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController(IMediator _mediator) : ControllerBase
{
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetByProfileId([FromRoute]string id, CancellationToken cancellationToken)
    {
        var query = new GetMatchesByProfileIdQuery(id);

        var matches = await _mediator.Send(query, cancellationToken);

        return Ok(matches);
    }
    
    [HttpGet("paged/{profileId}")]
    public async Task<IActionResult> GetPagedMatches([FromRoute] string profileId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetPagedMatchesQuery(profileId, pageNumber, pageSize);

        var matches = await _mediator.Send(query, cancellationToken);

        return Ok(matches);
    }
}