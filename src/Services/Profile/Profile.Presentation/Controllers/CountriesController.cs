using MediatR;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAll;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;
using Profile.Application.UseCases.CountryUseCases.Queries.GetById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
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
        var countries = await _mediator.Send(new GetAllCountriesQuery(), cancellationToken);
        
        return Ok(countries);
    }
    
    [HttpGet("{id}/cities")]
    public async Task<IActionResult> GetCitiesFromCountry([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetAllCitiesFromCountryQuery(id);
        
        var countries = await _mediator.Send(query, cancellationToken);
        
        return Ok(countries);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountryById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var country = await _mediator.Send(new GetCountryByIdQuery(id), cancellationToken);
        
        return Ok(country);
    }
}