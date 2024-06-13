using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService _userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRequestDto model)
    {
        var response = await _userService.RegisterAsync(model);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _userService.LoginAsync(model, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("assign/role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _userService.AssignRoleAsync(model.Email, model.Role, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("remove/from/role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveFromRole([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _userService.RemoveUserFromRoleAsync(model.Email, model.Role, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(RefreshRequestDto refreshToken, CancellationToken cancellationToken)
    {
        var response = await _userService.RefreshToken(refreshToken.refreshToken, cancellationToken);

        return Ok(response);
    }
}