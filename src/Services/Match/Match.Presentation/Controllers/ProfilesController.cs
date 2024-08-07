using Microsoft.AspNetCore.Mvc;
using Match.Application.DTOs.Profile.Request;
using Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;
using Match.Application.UseCases.ProfileUseCases.Queries.GetRecs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;

namespace Match.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
public class ProfilesController(IMediator _mediator) : ControllerBase
{


    [HttpGet("{profileId}/recommendations")]
    public async Task<IActionResult> GetPagedRecommendations([FromRoute] string profileId, CancellationToken cancellationToken)
    {
        var query = new GetRecsQuery(profileId);

        var profiles = await _mediator.Send(query, cancellationToken);

        return Ok(profiles);
    }
    
        
    [HttpPatch("location")]
    public async Task<IActionResult> UpdateLocation([FromBody]UpdateLocationDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateLocationCommand(dto);

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}