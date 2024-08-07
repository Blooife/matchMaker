using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Profile.Request;
using Profile.Application.UseCases.ProfileUseCases.Commands.Create;
using Profile.Application.UseCases.ProfileUseCases.Commands.Update;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetById;
using Profile.Application.UseCases.ProfileUseCases.Queries.GetByUserId;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]

public class ProfilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfileById([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new GetProfileByIdQuery(id);

        var profile = await _mediator.Send(query, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetProfileByUserId([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new GetProfileByUserIdQuery(id);

        var profile = await _mediator.Send(query, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateProfileCommand(dto);
        
        var profile = await _mediator.Send(command, cancellationToken);
        
        return Ok(profile);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateProfileCommand(dto);
        
        var profile = await _mediator.Send(command, cancellationToken);
        
        return Ok(profile);
    }
}