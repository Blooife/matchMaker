using Authentication.BusinessLogic.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Constants;
using Shared.Models;

namespace Authentication.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService _userService, IMapper _mapper): ControllerBase
{
    [HttpGet("{userId}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
    public async Task<IActionResult> GetUserById([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(userId, cancellationToken);
        
        return Ok(user);
    } 
    
    [HttpGet("email/{email}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
    public async Task<IActionResult> GetUserByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByEmailAsync(email, cancellationToken);
        
        return Ok(user);
    } 
    
    [HttpGet("paginated")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}")]
    public async Task<IActionResult> GetPaginatedUsers([FromQuery] int pageSize, [FromQuery] int pageNumber, CancellationToken cancellationToken)
    {
        var pagedList = await _userService.GetPaginatedUsersAsync(pageSize, pageNumber);
        var metadata = _mapper.Map<PaginationMetadata>(pagedList);

        HttpContext.Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        
        return Ok(pagedList);
    } 
    
    [HttpDelete("{userId}")]
    [Authorize(Roles = $"{Roles.Admin}, {Roles.Moderator}, {Roles.User}")]
    public async Task<IActionResult> DeleteUserById([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var users = await _userService.DeleteUserByIdAsync(userId, cancellationToken);
        
        return Ok(users);
    } 
}