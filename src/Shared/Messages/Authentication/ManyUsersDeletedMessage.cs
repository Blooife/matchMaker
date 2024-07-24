namespace Shared.Messages.Authentication;

public class ManyUsersDeletedMessage : BaseMessage
{
    public List<string> UsersIds { get; set; }

    public ManyUsersDeletedMessage(List<string> usersIds)
    {
        UsersIds = usersIds;
        Id = Guid.NewGuid().ToString();
    }
}