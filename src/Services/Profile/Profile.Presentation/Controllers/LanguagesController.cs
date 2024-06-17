using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;
using Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetAll;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetById;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetUsersLanguages;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LanguagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LanguagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllLanguages(CancellationToken cancellationToken)
    {
        var languages = await _mediator.Send(new GetAllLanguagesQuery(), cancellationToken);
        
        return Ok(languages);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLanguageById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var language = await _mediator.Send(new GetLanguageByIdQuery(id), cancellationToken);
        
        return Ok(language);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserLanguages(string id, CancellationToken cancellationToken)
    {
        var languages = await _mediator.Send(new GetUsersLanguagesQuery(id), cancellationToken);
        
        return Ok(languages);
    }

    [HttpPost("add/to/profile")]
    public async Task<IActionResult> AddLanguageToProfile([FromBody] AddLanguageToProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddLanguageToProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("remove/from/profile")]
    public async Task<IActionResult> RemoveLanguageFromProfile([FromBody] RemoveLanguageFromProfileDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveLanguageFromProfileCommand(dto), cancellationToken);
        
        return Ok(result);
    }
}