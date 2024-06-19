using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Match.Domain.Models;
using Match.Domain.Repositories;

namespace Match.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfilesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var profile = await _unitOfWork.Profiles.GetByIdAsync(id, HttpContext.RequestAborted);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Profile profile)
        {
            _unitOfWork.Profiles.Create(profile);
            return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Profile profile, CancellationToken cancellationToken)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }

            await _unitOfWork.Profiles.UpdateAsync(profile, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var existingProfile = await _unitOfWork.Profiles.GetByIdAsync(id, HttpContext.RequestAborted);
            if (existingProfile == null)
            {
                return NotFound();
            }

            await _unitOfWork.Profiles.DeleteAsync(existingProfile, cancellationToken);
            return NoContent();
        }
    }
}
