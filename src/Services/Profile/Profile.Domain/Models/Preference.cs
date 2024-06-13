using Shared.Constants;

namespace Profile.Domain.Models;

public class Preference
{
    public string ProfileId { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public int MaxDistance { get; set; }
    public Gender Gender { get; set; }
    public bool IsActive { get; set; }
    public UserProfile Profile { get; set; }

    public Preference()
    {
        AgeFrom = -1;
        AgeTo = -1;
        MaxDistance = -1;
        Gender = Gender.Undefined;
        IsActive = false;
    }
}