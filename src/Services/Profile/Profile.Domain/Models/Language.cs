namespace Profile.Domain.Models;

public class Language
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<UserProfile> Profiles { get; set; } = new List<UserProfile>();
}