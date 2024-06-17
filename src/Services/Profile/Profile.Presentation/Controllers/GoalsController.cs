using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Goal.Request;
using Profile.Application.UseCases.GoalUseCases.Commands.AddGoalToProfile;
using Profile.Application.UseCases.GoalUseCases.Commands.RemoveGoalFromProfile;
using Profile.Application.UseCases.GoalUseCases.Queries.GetAll;
using Profile.Application.UseCases.GoalUseCases.Queries.GetById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        var goals = await _mediator.Send(new GetAllGoalsQuery(), cancellationToken);
        
        return Ok(goals);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var goal = await _mediator.Send(new GetGoalByIdQuery(id), cancellationToken);
        
        return Ok(goal);
    }

    [HttpPost("add/to/goal")]
    public async Task<IActionResult> AddGoalToProfile([FromBody] AddGoalToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddGoalToProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("remove/from/goal")]
    public async Task<IActionResult> RemoveGoalFromProfile([FromBody] RemoveGoalFromProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveGoalFromProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
}