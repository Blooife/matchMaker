using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;
using Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;
using Profile.Application.UseCases.InterestUseCases.Queries.GetAll;
using Profile.Application.UseCases.InterestUseCases.Queries.GetById;
using Profile.Application.UseCases.InterestUseCases.Queries.GetUsersInterests;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        var interests = await _mediator.Send(new GetAllInterestsQuery(), cancellationToken);
        
        return Ok(interests);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInterestById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var interest = await _mediator.Send(new GetInterestByIdQuery(id), cancellationToken);
        
        return Ok(interest);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserInterests(string id, CancellationToken cancellationToken)
    {
        var interests = await _mediator.Send(new GetUsersInterestsQuery(id), cancellationToken);
        
        return Ok(interests);
    }

    [HttpPost("add/to/profile")]
    public async Task<IActionResult> AddInterestToProfile([FromBody] AddInterestToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddInterestToProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("remove/from/profile")]
    public async Task<IActionResult> RemoveInterestFromProfile([FromBody] RemoveInterestFromProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveInterestFromProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
}