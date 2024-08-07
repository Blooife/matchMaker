using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;
using Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetAll;
using Profile.Application.UseCases.LanguageUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
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
        var query = new GetAllLanguagesQuery();

        var languages = await _mediator.Send(query, cancellationToken);
        
        return Ok(languages);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLanguageById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetLanguageByIdQuery(id);

        var language = await _mediator.Send(query, cancellationToken);
        
        return Ok(language);
    }

    [HttpPost("profile")]
    public async Task<IActionResult> AddLanguageToProfile([FromBody] AddLanguageToProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new AddLanguageToProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("profile")]
    public async Task<IActionResult> RemoveLanguageFromProfile([FromBody] RemoveLanguageFromProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveLanguageFromProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}