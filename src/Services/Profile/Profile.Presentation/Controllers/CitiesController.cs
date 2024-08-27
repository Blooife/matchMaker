using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.UseCases.CityUseCases.Queries.GetAll;
using Profile.Application.UseCases.CityUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
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
}