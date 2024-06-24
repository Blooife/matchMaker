using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRequestDto model)
    {
        var response = await _authService.RegisterAsync(model);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(model, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("assign/role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _authService.AssignRoleAsync(model.Email, model.Role, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpDelete("remove/from/role")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveFromRole([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _authService.RemoveUserFromRoleAsync(model.Email, model.Role, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto refreshToken, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshToken(refreshToken.refreshToken, cancellationToken);

        return Ok(response);
    }
}