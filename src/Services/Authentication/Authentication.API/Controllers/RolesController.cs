using Authentication.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService _roleService): ControllerBase
{
    [HttpGet]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetAllRolesAsync(cancellationToken);
        return Ok(roles);
    } 
}