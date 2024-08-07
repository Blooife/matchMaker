using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.Image.Request;
using Profile.Application.Services.Interfaces;
using Profile.Application.UseCases.ImageUseCases.Commands.AddImage;
using Profile.Application.UseCases.ImageUseCases.Commands.ChangeMainImage;
using Profile.Application.UseCases.ImageUseCases.Commands.RemoveImage;
using Profile.Application.UseCases.ImageUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
public class ImagesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMinioService _minioService;

    public ImagesController(IMediator mediator, IMinioService minioService)
    {
        _mediator = mediator;
        _minioService = minioService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(int id, CancellationToken cancellationToken)
    {
        var query = new GetImageByIdQuery(id);

        var image = await _mediator.Send(query, cancellationToken);

        return Ok(image);
    }
    
    [HttpGet("file/{id}")]
    public async Task<IActionResult> GetImageFileById(int id, CancellationToken cancellationToken)
    {
        var query = new GetImageByIdQuery(id);

        var image = await _mediator.Send(query, cancellationToken);
        var stream = await _minioService.GetFileAsync(image.ImageUrl);
        
        Response.Headers.Append("Content-Disposition", "inline");
        
        return File(stream, GetMimeType(image.ImageUrl));
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
    
    private string GetMimeType(string fileName)
    {
        var mimeTypes = new Dictionary<string, string>
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
        };

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        
        return mimeTypes.ContainsKey(extension) ? mimeTypes[extension] : "application/octet-stream";
    }
}