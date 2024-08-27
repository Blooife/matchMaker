using Shared.Constants;

namespace Shared.Messages.Profile;

public class ProfileUpdatedMessage : BaseMessage
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string MainImageUrl { get; set; }
}