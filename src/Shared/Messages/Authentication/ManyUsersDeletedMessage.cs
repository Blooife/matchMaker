namespace Shared.Messages.Authentication;

public class ManyUsersDeletedMessage : BaseMessage
{
    public IEnumerable<string> UsersIds { get; set; }

    public ManyUsersDeletedMessage(IEnumerable<string> usersIds)
    {
        UsersIds = usersIds;
        Id = Guid.NewGuid().ToString();
    }
}