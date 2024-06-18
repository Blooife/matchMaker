using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;
using Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;
using Profile.Application.UseCases.EducationUseCases.Commands.Update;
using Profile.Application.UseCases.EducationUseCases.Queries.GetAll;
using Profile.Application.UseCases.EducationUseCases.Queries.GetById;
using Profile.Application.UseCases.EducationUseCases.Queries.GetUsersEducation;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EducationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EducationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEducations(CancellationToken cancellationToken)
    {
        var education = await _mediator.Send(new GetAllEducationsQuery(), cancellationToken);
        
        return Ok(education);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var education = await _mediator.Send(new GetEducationByIdQuery(id), cancellationToken);
        
        return Ok(education);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserEducations(string id, CancellationToken cancellationToken)
    {
        var education = await _mediator.Send(new GetUsersEducationsQuery(id), cancellationToken);
        
        return Ok(education);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateUserEducation([FromBody] UpdateUserEducationDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateUserEducationCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost("add/to/profile")]
    public async Task<IActionResult> AddEducationToProfile([FromBody] AddEducationToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddEducationToProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("remove/from/profile")]
    public async Task<IActionResult> RemoveEducationFromProfile([FromBody] RemoveEducationFromProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveEducationFromProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
}