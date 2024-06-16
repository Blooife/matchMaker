namespace Authentication.BusinessLogic.DTOs.Response;

public class UserResponseDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
}