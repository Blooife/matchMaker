namespace Profile.Domain.Models;

public class City : BaseModel<int>
{
    public string Name { get; set; }
    
    public int CountryId { get; set; }
    public Country Country { get; set; }

    public List<UserProfile> Profiles { get; set; } = new List<UserProfile>();
}