using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.UseCases.ImageUseCases.Commands.AddImage;
using Profile.Application.UseCases.ImageUseCases.Commands.ChangeMainImage;
using Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;
using Profile.Application.UseCases.ImageUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(int id, CancellationToken cancellationToken)
    {
        var query = new GetImageByIdQuery(id);

        var image = await _mediator.Send(query, cancellationToken);

        return Ok(image);
    }
    
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddImageToProfile(AddImageDto dto, CancellationToken cancellationToken)
    {
        var command = new AddImageCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPatch]
    public async Task<IActionResult> ChangeMainImage(ChangeMainImageDto dto, CancellationToken cancellationToken)
    {
        var command = new ChangeMainImageCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> RemoveImageFromProfile([FromBody] RemoveImageDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveImageCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}