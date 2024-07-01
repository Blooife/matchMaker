using Shared.Constants;

namespace Match.Application.DTOs.Profile.Response;

public class FullProfileResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public string? Bio { get; set; }
    public int? Height { get; set; }
    public bool ShowAge { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Goal { get; set; }
    public List<string> Languages { get; set; } = new List<string>();
    public List<string> Interests { get; set; } = new List<string>();
    public List<string> Images { get; set; } = new List<string>();
}