using Authentication.BusinessLogic.DTOs;
using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IAuthService
{
    Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto);
    Task<LoginResponseDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken);
    Task<GeneralResponseDto> AssignRoleAsync(string name, string roleName, CancellationToken cancellationToken);
    Task<GeneralResponseDto> RemoveUserFromRoleAsync(string userId, string roleName, CancellationToken cancellationToken);
    Task<LoginResponseDto> RefreshToken(string refreshToken, CancellationToken cancellationToken);
}