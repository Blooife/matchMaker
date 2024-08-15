namespace Profile.Domain.Models;

public class Education : BaseModel<int>
{
    public string Name { get; set; }
    public List<ProfileEducation> ProfileEducations { get; set; } = new List<ProfileEducation>();
}