namespace Authentication.BusinessLogic.DTOs.Response;

public class LoginResponseDto
{
    public UserResponseDto userDto { get; set; }
    public string JwtToken { get; set; }
}