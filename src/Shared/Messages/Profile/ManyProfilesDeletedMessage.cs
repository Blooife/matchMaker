namespace Shared.Messages.Profile;

public class ManyProfilesDeletedMessage : BaseMessage
{
    private IEnumerable<string> ProfilesIds { get; set; }

    public ManyProfilesDeletedMessage(IEnumerable<string> profilesIds)
    {
        ProfilesIds = profilesIds;
        Id = Guid.NewGuid().ToString();
    }
}