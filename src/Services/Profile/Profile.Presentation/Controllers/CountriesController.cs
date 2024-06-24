using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAll;
using Profile.Application.UseCases.CountryUseCases.Queries.GetAllCitiesFromCountry;
using Profile.Application.UseCases.CountryUseCases.Queries.GetById;

namespace Profile.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
        
        var countries = await _mediator.Send(query, cancellationToken);
        
        return Ok(countries);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCountryById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetCountryByIdQuery(id);
        
        var country = await _mediator.Send(query, cancellationToken);
        
        return Ok(country);
    }
}