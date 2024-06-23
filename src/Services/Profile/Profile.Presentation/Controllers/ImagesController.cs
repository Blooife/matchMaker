using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.UseCases.ImageUseCases.Commands.AddImage;
using Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;
using Profile.Application.UseCases.ImageUseCases.Queries.GetProfilesImages;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetProfileImages(string id, CancellationToken cancellationToken)
    {
        var query = new GetProfilesImagesQuery(id);

        var images = await _mediator.Send(query, cancellationToken);
        
        return Ok(images);
    }

    [HttpPost("to/profile")]
    public async Task<IActionResult> AddImageToProfile(AddImageDto dto, CancellationToken cancellationToken)
    {
        var command = new AddImageCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("from/profile")]
    public async Task<IActionResult> RemoveImageFromProfile([FromBody] RemoveImageDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveImageCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}