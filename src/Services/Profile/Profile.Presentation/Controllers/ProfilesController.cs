using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Profile.Request;
using Profile.Application.UseCases.ProfileUseCases.Commands.Create;
using Profile.Application.UseCases.ProfileUseCases.Commands.Delete;
using Profile.Application.UseCases.ProfileUseCases.Commands.Update;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetAll;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var profiles = await _mediator.Send(new GetAllProfilesQuery(), cancellationToken);
        return Ok(profiles);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfileById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var profile = await _mediator.Send(new GetProfileByIdQuery(id), cancellationToken);
        return Ok(profile);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDto dto, CancellationToken cancellationToken)
    {
        var profile = await _mediator.Send(new CreateProfileCommand(dto), cancellationToken);
        return Ok(profile);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto, CancellationToken cancellationToken)
    {
        var profile = await _mediator.Send(new UpdateProfileCommand(dto), cancellationToken);
        return Ok(profile);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> UpdateProfile([FromRoute] string id, CancellationToken cancellationToken)
    {
        var profile = await _mediator.Send(new DeleteProfileCommand(id), cancellationToken);
        return Ok(profile);
    }
}