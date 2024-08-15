using Shared.Interfaces;

namespace Profile.Domain.Models;

public class User : BaseModel<string>, ISoftDeletable
{
    public string Email { get; set; }
    public DateTime? DeletedAt { get; set; }
    public UserProfile Profile { get; set; }
}