namespace Profile.Domain.Models;

public class User
{
    public string Id { get; set; }
    public string Email { get; set; }
    
    public UserProfile Profile { get; set; }
}