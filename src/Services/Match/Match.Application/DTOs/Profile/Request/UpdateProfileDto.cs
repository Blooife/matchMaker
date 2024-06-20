using Shared.Constants;

namespace Match.Application.DTOs.Profile.Request;

public class UpdateProfileDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string UserLastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
}