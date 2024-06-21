namespace Profile.Domain.Models;

public class ProfileEducation
{
    public string ProfileId { get; set; }
    public UserProfile Profile { get; set; }
    
    public int EducationId { get; set; }
    public Education Education { get; set; }
    
    public string Description { get; set; }
}