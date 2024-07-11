using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto refreshToken, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(refreshToken.refreshToken, cancellationToken);

        return Ok(response);
    }
}