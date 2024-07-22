namespace Shared.Messages.Authentication;

public class UserCreatedMessage : BaseMessage
{
    public string Email { get; set; }
}