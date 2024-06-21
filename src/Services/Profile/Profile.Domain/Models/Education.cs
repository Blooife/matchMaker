namespace Profile.Domain.Models;

public class Education
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ProfileEducation> ProfileEducations { get; set; } = new List<ProfileEducation>();
}