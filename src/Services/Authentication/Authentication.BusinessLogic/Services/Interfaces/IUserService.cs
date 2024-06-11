using Authentication.BusinessLogic.DTOs;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto);
    Task<UserDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken);
    Task<GeneralResponseDto> AssignRoleAsync(string name, string roleName, CancellationToken cancellationToken);
    Task<GeneralResponseDto> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<GeneralResponseDto> RemoveUserFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
    Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<RoleResponseDto>> GetUsersRoles(string userId, CancellationToken cancellationToken);
    Task<UserDto> RefreshToken(string refrToken, CancellationToken cancellationToken);
}