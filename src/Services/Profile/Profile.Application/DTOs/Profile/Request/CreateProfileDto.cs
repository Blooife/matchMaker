using Shared.Constants;

namespace Profile.Application.DTOs.Profile.Request;

public class CreateProfileDto
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; } = Gender.Undefined;
    public string? Bio { get; set; }
    public int? Height { get; set; }
    public DateTime LastOnline { get; set; }
    public bool ShowAge { get; set; } = false;
    public int? GoalId { get; set; }
    public int? CityId { get; set; }
}