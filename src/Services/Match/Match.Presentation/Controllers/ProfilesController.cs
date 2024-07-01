using Microsoft.AspNetCore.Mvc;
using Match.Application.DTOs.Profile.Request;
using Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;
using Match.Application.UseCases.ProfileUseCases.Queries.GetById;
using Match.Application.UseCases.ProfileUseCases.Queries.GetPagedRecs;
using Match.Application.UseCases.ProfileUseCases.Queries.GetRecsByProfileId;
using MediatR;

namespace Match.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController(IMediator _mediator) : ControllerBase
    {

        [HttpGet("{profileId}/recommendations")]
        public async Task<IActionResult> GetRecommendations([FromRoute]string profileId, CancellationToken cancellationToken)
        {
            var query = new GetRecsByProfileIdQuery(profileId);

            var recommendations = await _mediator.Send(query, cancellationToken);

            return Ok(recommendations);
        }
        
        [HttpGet("{profileId}/paged/recommendations")]
        public async Task<IActionResult> GetPagedRecommendations([FromRoute] string profileId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedRecsQuery(profileId, pageNumber, pageSize);

            var profiles = await _mediator.Send(query, cancellationToken);

            return Ok(profiles);
        }
        
        [HttpGet("{profileId}")]
        public async Task<IActionResult> GetById([FromRoute] string profileId, CancellationToken cancellationToken)
        {
            var query = new GetProfileByIdQuery(profileId);

            var profiles = await _mediator.Send(query, cancellationToken);

            return Ok(profiles);
        }
        
        [HttpPatch("location")]
        public async Task<IActionResult> UpdateLocation([FromBody]UpdateLocationDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateLocationCommand(dto);

            var profile = await _mediator.Send(command, cancellationToken);

            return Ok(profile);
        }
    }
}
