using Microsoft.AspNetCore.Identity;

namespace Authentication.DataLayer.Models;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiredAt { get; set; }
}