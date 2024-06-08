using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.Request;
using BusinessLogic.DTOs.Response;
using BusinessLogic.Exceptions;
using BusinessLogic.Services.Interfaces;
using DataLayer.Models;
using DataLayer.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations;

public class AuthService(IUserRepository _userRepository, IMapper _mapper) : IAuthService
{
    public async Task<GeneralResponseDto> RegisterAsync(UserRequestDto registrationRequestDto, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(registrationRequestDto);
        
        var result = await _userRepository.RegisterAsync(user, registrationRequestDto.Password);
        if (result.Succeeded)
        {
            return new  GeneralResponseDto() { Message = "User registered successfully"};
        }
        
        throw new RegisterException(result.Errors.FirstOrDefault().Description);
        
    }

    public async Task<UserDto> LoginAsync(UserRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequestDto.Email, cancellationToken);
            
        if (user == null)
        {
            throw new LoginException(ExceptionMessages.LoginFailed);
        }

        var isValid = await _userRepository.CheckPasswordAsync(user, loginRequestDto.Password);

        if (isValid == false)
        {
            throw new LoginException(ExceptionMessages.LoginFailed);
        }
            
        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }

    public async Task<GeneralResponseDto> AssignRoleAsync(string name, string roleName, CancellationToken cancellationToken)
    {
        return new GeneralResponseDto();
    }

    public async Task<GeneralResponseDto> CreateRoleAsync(string roleName)
    {
        return new GeneralResponseDto();
    }
}