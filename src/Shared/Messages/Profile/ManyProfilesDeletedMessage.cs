namespace Shared.Messages.Profile;

public class ManyProfilesDeletedMessage : BaseMessage
{
    public List<string> ProfilesIds { get; set; }

    public ManyProfilesDeletedMessage(List<string> profilesIds)
    {
        ProfilesIds = profilesIds;
        Id = Guid.NewGuid().ToString();
    }
}