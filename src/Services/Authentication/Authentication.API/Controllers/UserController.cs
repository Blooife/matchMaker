using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;

namespace Authentication.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService _userService): ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRequestDto model)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        var response = await _userService.RegisterAsync(model);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ModelState);
        }
        var response = await _userService.LoginAsync(model, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("assignRole")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignRoleAsync([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _userService.AssignRoleAsync(model.Email, model.Role, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("removeFromRole")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveFromRoleAsync([FromBody] AssignRoleRequestDto model, CancellationToken cancellationToken)
    {
        var response = await _userService.RemoveUserFromRoleAsync(model.Email, model.Role, cancellationToken);
        return Ok(response);
    }
    
    [HttpDelete("{userId}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var response = await _userService.DeleteUserByIdAsync(userId, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(string userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        return Ok(user);
    } 
    
    [HttpGet("getByEmail/{email}")]
    [Authorize]
    public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmailAsync(email, cancellationToken);
        return Ok(user);
    } 
    
    [HttpGet("getRoles/{userId}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> GetUsersRoles(string userId, CancellationToken cancellationToken)
    {
        var roles = await _userService.GetUsersRoles(userId, cancellationToken);
        return Ok(roles);
    } 
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllUsersAsync(cancellationToken);
        return Ok(users);
    } 
    
    [HttpPost("refreshToken")]
    [Authorize]
    public async Task<IActionResult> RefreshToken(RefreshRequestDto refreshToken, CancellationToken cancellationToken)
    {
        var response = await _userService.RefreshToken(refreshToken.refreshToken, cancellationToken);

        return Ok(response);
    }
}