using Shared.Constants;

namespace Profile.Domain.Models;

public class UserProfile
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
    
    public Preference Preference { get; set; }
    
    public int? GoalId { get; set; }
    public Goal? Goal { get; set; }
    
    public int? CityId { get; set; }
    public City City { get; set; }

    public List<Language> Languages { get; set; } = new List<Language>();
    public List<Interest> Interests { get; set; } = new List<Interest>();
    public List<UserEducation> UserEducations { get; set; } = new List<UserEducation>();

}