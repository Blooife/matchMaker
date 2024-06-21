using Shared.Constants;

namespace Profile.Application.DTOs.Profile.Request;

public class UpdateProfileDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public int? Height { get; set; }
    public bool ShowAge { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public int MaxDistance { get; set; }
    public Gender PreferredGender { get; set; } = Gender.Undefined;
    public int? GoalId { get; set; }
    public int CityId { get; set; }
    public int ProfileId { get; set; }
}