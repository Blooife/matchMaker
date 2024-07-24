using Authentication.BusinessLogic.DTOs.Response;
using Shared.Models;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IUserService
{
    Task<GeneralResponseDto> DeleteUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    Task<UserResponseDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<IEnumerable<RoleResponseDto>> GetUsersRolesAsync(string userId, CancellationToken cancellationToken);
    Task<PagedList<UserResponseDto>> GetPaginatedUsersAsync(int pageSize, int pageNumber);
}