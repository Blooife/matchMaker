namespace Authentication.BusinessLogic.DTOs.Response;

public class LoginResponseDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
    public string JwtToken { get; set; }
}