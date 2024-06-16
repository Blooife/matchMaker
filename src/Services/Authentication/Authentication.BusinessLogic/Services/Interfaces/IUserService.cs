using Authentication.BusinessLogic.DTOs;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<GeneralResponseDto> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<RoleResponseDto>> GetUsersRoles(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserResponseDto>>
        GetPaginatedUsersAsync(int pageSize, int pageNumber, CancellationToken cancellationToken);

    Task<GeneralResponseDto> UpdateUser(UserRequestDto userDto);
}