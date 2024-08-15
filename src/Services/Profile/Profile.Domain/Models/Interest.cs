namespace Profile.Domain.Models;

public class Interest : BaseModel<int>
{
    public string Name { get; set; }

    public List<UserProfile> Profiles { get; set; } = new List<UserProfile>();
}