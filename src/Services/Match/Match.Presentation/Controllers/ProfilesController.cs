using Microsoft.AspNetCore.Mvc;
using Match.Application.DTOs.Profile.Request;
using Match.Application.UseCases.ProfileUseCases.Commands.Create;
using Match.Application.UseCases.ProfileUseCases.Commands.Update;
using Match.Application.UseCases.ProfileUseCases.Commands.UpdateLocation;
using Match.Application.UseCases.ProfileUseCases.Queries.GetPagedRecs;
using Match.Application.UseCases.ProfileUseCases.Queries.GetRecsByProfileId;
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
        
        [HttpGet("paged/{profileId}")]
        public async Task<IActionResult> GetPagedRecommendations([FromRoute] string profileId, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPagedRecsQuery(profileId, pageNumber, pageSize);

            var profiles = await _mediator.Send(query, cancellationToken);

            return Ok(profiles);
        }
        
        [HttpPut("location")]
        public async Task<IActionResult> UpdateLocation([FromBody]UpdateLocationDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateLocationCommand(dto);

            var profile = await _mediator.Send(command, cancellationToken);

            return Ok(profile);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileDto dto, CancellationToken cancellationToken)
        {
            var query = new CreateProfileCommand(dto);

            var recs = await _mediator.Send(query, cancellationToken);

            return Ok(recs);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileDto dto, CancellationToken cancellationToken)
        {
            var query = new UpdateProfileCommand(dto);

            var recs = await _mediator.Send(query, cancellationToken);

            return Ok(recs);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile([FromRoute]string id, CancellationToken cancellationToken)
        {
            var query = new GetRecsByProfileIdQuery(id);

            var recs = await _mediator.Send(query, cancellationToken);

            return Ok(recs);
        }
        
    }
}
