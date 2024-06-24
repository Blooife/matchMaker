using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;
using Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;
using Profile.Application.UseCases.EducationUseCases.Commands.Update;
using Profile.Application.UseCases.EducationUseCases.Queries.GetAll;
using Profile.Application.UseCases.EducationUseCases.Queries.GetById;
using Profile.Application.UseCases.EducationUseCases.Queries.GetProfilesEducation;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        var query = new GetAllEducationsQuery();
            
        var education = await _mediator.Send(query, cancellationToken);
        
        return Ok(education);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEducationById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetEducationByIdQuery(id);
            
        var education = await _mediator.Send(query, cancellationToken);
        
        return Ok(education);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetProfileEducations(string id, CancellationToken cancellationToken)
    {
        var query = new GetProfilesEducationsQuery(id);

        var education = await _mediator.Send(query, cancellationToken);
        
        return Ok(education);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfileEducation([FromBody] UpdateProfileEducationDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateProfileEducationCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }

    [HttpPost("to/profile")]
    public async Task<IActionResult> AddEducationToProfile([FromBody] AddEducationToProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new AddEducationToProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("from/profile")]
    public async Task<IActionResult> RemoveEducationFromProfile([FromBody] RemoveEducationFromProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveEducationFromProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}