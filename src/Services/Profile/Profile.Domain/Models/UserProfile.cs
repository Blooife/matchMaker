using Shared.Constants;
using Shared.Interfaces;

namespace Profile.Domain.Models;

public class UserProfile : ISoftDeletable
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
    public string UserId { get; set; }
    public int? GoalId { get; set; }
    public int CityId { get; set; }
    
    public City City { get; set; }
    public User User { get; set; }
    public Goal? Goal { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Language> Languages { get; set; } = new List<Language>();
    public List<Interest> Interests { get; set; } = new List<Interest>();
    public List<ProfileEducation> ProfileEducations { get; set; } = new List<ProfileEducation>();
    public List<Image> Images { get; set; } = new List<Image>();

}