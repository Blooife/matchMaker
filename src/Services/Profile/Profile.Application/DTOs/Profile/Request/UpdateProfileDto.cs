using Shared.Constants;

namespace Profile.Application.DTOs.Profile.Request;

public class UpdateProfileDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? Bio { get; set; }
    public int Height { get; set; }
    public DateTime LastOnline { get; set; }
    public bool ShowAge { get; set; }
    public int? GoalId { get; set; }
    public int? CityId { get; set; }
}