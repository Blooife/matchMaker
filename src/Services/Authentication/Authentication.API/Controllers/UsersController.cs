using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Constants;

namespace Authentication.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService _userService): ControllerBase
{
    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> GetUserById([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        
        return Ok(user);
    } 
    
    [HttpGet("get/by/email/{email}")]
    [Authorize]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmailAsync(email, cancellationToken);
        
        return Ok(user);
    } 
    
    [HttpGet("roles/{userId}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> GetUsersRoles([FromRoute] string userId, CancellationToken cancellationToken)
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
    
    [HttpGet("paginated/users")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> GetPaginatedUsers([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
    {
        var pagedList = await _userService.GetPaginatedUsersAsync(pageSize, pageNumber);
        var metadata = new
        {
            pagedList.TotalCount,
            pagedList.PageSize,
            pagedList.CurrentPage,
            pagedList.TotalPages,
            pagedList.HasNext,
            pagedList.HasPrevious
        };

        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        return Ok(pagedList);
    } 
    
    [HttpDelete("{userId}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    [Authorize]
    public async Task<IActionResult> DeleteUserById([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var users = await _userService.DeleteUserByIdAsync(userId, cancellationToken);
        
        return Ok(users);
    } 
    
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UserRequestDto userRequestDto)
    {
        var users = await _userService.UpdateUser(userRequestDto);
        
        return Ok(users);
    } 
}