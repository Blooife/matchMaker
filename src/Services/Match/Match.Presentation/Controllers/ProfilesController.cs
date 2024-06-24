using Microsoft.AspNetCore.Mvc;
using Match.Application.DTOs.Profile.Request;
using Match.Application.UseCases.ProfileUseCases.Commands.Create;
using Match.Application.UseCases.ProfileUseCases.Commands.Update;
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
