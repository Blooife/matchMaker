namespace Authentication.BusinessLogic.DTOs;

public class UserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
    public string? JwtToken { get; set; }
}