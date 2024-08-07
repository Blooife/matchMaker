using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;
using Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;
using Profile.Application.UseCases.InterestUseCases.Queries.GetAll;
using Profile.Application.UseCases.InterestUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
public class InterestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InterestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInterests(CancellationToken cancellationToken)
    {
        var query = new GetAllInterestsQuery();

        var interests = await _mediator.Send(query, cancellationToken);
        
        return Ok(interests);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInterestById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetInterestByIdQuery(id);

        var interest = await _mediator.Send(query, cancellationToken);
        
        return Ok(interest);
    }

    [HttpPost("profile")]
    public async Task<IActionResult> AddInterestToProfile([FromBody] AddInterestToProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new AddInterestToProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("profile")]
    public async Task<IActionResult> RemoveInterestFromProfile([FromBody] RemoveInterestFromProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveInterestFromProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}