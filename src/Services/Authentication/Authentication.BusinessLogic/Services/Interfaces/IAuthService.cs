using Authentication.BusinessLogic.DTOs.Request;
using Authentication.BusinessLogic.DTOs.Response;
using Shared.Models;

namespace Authentication.BusinessLogic.Services.Interfaces;

public interface IAuthService
{
    Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto);
    Task<LoginResponseDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}