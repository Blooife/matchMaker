using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.DTOs.City.Request;
using Profile.Application.UseCases.CityUseCases.Commands.AddCityToProfile;
using Profile.Application.UseCases.CityUseCases.Commands.RemoveCityFromProfile;
using Profile.Application.UseCases.CityUseCases.Queries.GetAll;
using Profile.Application.UseCases.CityUseCases.Queries.GetById;
using Profile.Application.UseCases.CityUseCases.Queries.GetCityWithCountryById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCities(CancellationToken cancellationToken)
    {
        var query = new GetAllCitiesQuery();
        var cities = await _mediator.Send(query, cancellationToken);
        
        return Ok(cities);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCityById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCityByIdQuery(id);
        
        var city = await _mediator.Send(query, cancellationToken);
        
        return Ok(city);
    }
    
    [HttpGet("{id}/with/country")]
    public async Task<IActionResult> GetCityWithCountryById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCityWithCountryByIdQuery(id);
        
        var city = await _mediator.Send(query, cancellationToken);
        
        return Ok(city);
    }

    [HttpPost("add/to/profile")]
    public async Task<IActionResult> AddCityToProfile([FromBody] AddCityToProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new AddCityToProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("remove/from/profile")]
    public async Task<IActionResult> RemoveCityFromProfile([FromBody] RemoveCityFromProfileDto dto, CancellationToken cancellationToken)
    {
        var command = new RemoveCityFromProfileCommand(dto);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return Ok(result);
    }
}