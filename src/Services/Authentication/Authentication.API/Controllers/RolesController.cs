using Authentication.BusinessLogic.DTOs.Request;
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
    //[Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await _roleService.GetAllRolesAsync(cancellationToken);
        
        return Ok(roles);
    } 
    
    [HttpPost("assignment")]
    //[Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _roleService.AssignRoleAsync(model.Email, model.Role, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpDelete("removal")]
    //[Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveFromRole([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _roleService.RemoveUserFromRoleAsync(model.Email, model.Role, cancellationToken);
        
        return Ok(response);
    }
}