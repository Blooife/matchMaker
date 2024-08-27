using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAll;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;
using Profile.Application.UseCases.CountryUseCases.Queries.GetById;
using Shared.Constants;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{Roles.User}")]
public class CountriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CountriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken)
    {
        var query = new GetAllCountriesQuery();
        
        var countries = await _mediator.Send(query, cancellationToken);
        
        return Ok(countries);
    }
    
    [HttpGet("{id}/cities")]
    public async Task<IActionResult> GetCitiesFromCountry([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetAllCitiesFromCountryQuery(id);
        
        var cities = await _mediator.Send(query, cancellationToken);
        
        return Ok(cities);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountryById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCountryByIdQuery(id);
        
        var country = await _mediator.Send(query, cancellationToken);
        
        return Ok(country);
    }
}