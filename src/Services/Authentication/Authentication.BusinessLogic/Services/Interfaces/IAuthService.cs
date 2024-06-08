using BusinessLogic.DTOs;
using BusinessLogic.DTOs.Request;
using BusinessLogic.DTOs.Response;

namespace BusinessLogic.Services.Interfaces;

public interface IAuthService
{
    Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto, CancellationToken cancellationToken);
    Task<UserDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken);
    Task<GeneralResponseDto> AssignRoleAsync(string name, string roleName, CancellationToken cancellationToken);
    Task<GeneralResponseDto> CreateRoleAsync(string roleName);
}