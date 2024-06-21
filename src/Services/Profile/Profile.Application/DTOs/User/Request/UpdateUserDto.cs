using Shared.Constants;

namespace Profile.Application.DTOs.User.Request;

public class UpdateUserDto
{
    public string Id { get; set; }
    public string Email { get; set; }
}