using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Goal.Request;
using Profile.Application.UseCases.GoalUseCases.Commands.AddGoalToProfile;
using Profile.Application.UseCases.GoalUseCases.Commands.RemoveGoalFromProfile;
using Profile.Application.UseCases.GoalUseCases.Queries.GetAll;
using Profile.Application.UseCases.GoalUseCases.Queries.GetById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GoalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGoals(CancellationToken cancellationToken)
    {
        var query = new GetAllGoalsQuery();

        var goals = await _mediator.Send(query, cancellationToken);
        
        return Ok(goals);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetGoalByIdQuery(id);

        var goal = await _mediator.Send(query, cancellationToken);
        
        return Ok(goal);
    }

    [HttpPost("to/profile")]
    public async Task<IActionResult> AddGoalToProfile([FromBody] AddGoalToProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new AddGoalToProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("from/profile")]
    public async Task<IActionResult> RemoveGoalFromProfile([FromBody] RemoveGoalFromProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveGoalFromProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}