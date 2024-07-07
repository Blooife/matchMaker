using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Authentication.DataLayer.Models;
using Shared.Models;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<GeneralResponseDto> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<RoleResponseDto>> GetUsersRolesAsync(string userId, CancellationToken cancellationToken);
    Task<PagedList<User>> GetPaginatedUsersAsync(int pageSize, int pageNumber);
}