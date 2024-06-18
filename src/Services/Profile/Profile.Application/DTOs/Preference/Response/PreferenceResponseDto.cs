using Shared.Constants;

namespace Profile.Application.DTOs.Preference.Response;

public class PreferenceResponseDto
{
    public string ProfileId { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public int MaxDistance { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
}