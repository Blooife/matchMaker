using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Preference.Request;
using Profile.Application.UseCases.PreferenceUseCases.Commands.ChangeIsActive;
using Profile.Application.UseCases.PreferenceUseCases.Commands.Update;
using Profile.Application.UseCases.PreferenceUseCases.Queries.GetById;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PreferencesController(IMediator _mediator) : ControllerBase
{
    [HttpGet("{profileId}")]
    public async Task<IActionResult> GetPreferenceById([FromRoute] string profileId, CancellationToken cancellationToken)
    {
        var query = new GetPreferenceByIdQuery(profileId);

        var preference = await _mediator.Send(query, cancellationToken);

        return Ok(preference);
    }

    [HttpPut("change/is/active/{profileId}")]
    public async Task<IActionResult> ChangeIsActive([FromRoute] string profileId, CancellationToken cancellationToken)
    {
        var command = new ChangeIsActiveCommand(profileId);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePreference([FromBody] UpdatePreferenceDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdatePreferenceCommand(dto);

        var preference = await _mediator.Send(command, cancellationToken);

        return Ok(preference);
    }
}