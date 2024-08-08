using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.UseCases.GoalUseCases.Queries.GetAll;
using Profile.Application.UseCases.GoalUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
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
}