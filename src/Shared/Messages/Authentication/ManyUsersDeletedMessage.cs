using Shared.Messages.Authentication;

namespace Shared.Messages.Authentication;

public class ManyUsersDeletedMessage : BaseMessage
{
    private IEnumerable<string> UsersIds { get; set; }

    public ManyUsersDeletedMessage(IEnumerable<string> usersIds)
    {
        UsersIds = usersIds;
        Id = Guid.NewGuid().ToString();
    }
}