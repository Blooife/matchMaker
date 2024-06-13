namespace Profile.Domain.Models;

public class Education
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserEducation> UserEducations { get; set; } = new List<UserEducation>();
}