using Shared.Constants;

namespace Match.Application.DTOs.Profile.Response;

public class ProfileResponseDto
{
    public string Id { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; } 
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
}