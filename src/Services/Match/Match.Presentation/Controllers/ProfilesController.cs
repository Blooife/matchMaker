using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Match.Application.UseCases.ProfileUseCases.Queries.GetRecsByProfileId;
using Match.Domain.Models;
using Match.Domain.Repositories;
using MediatR;

namespace Match.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController(IMediator _mediator) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecommendations([FromRoute]string id, CancellationToken cancellationToken)
        {
            var query = new GetRecsByProfileIdQuery(id);

            var recs = await _mediator.Send(query, cancellationToken);

            return Ok(recs);
        }
    }
}
