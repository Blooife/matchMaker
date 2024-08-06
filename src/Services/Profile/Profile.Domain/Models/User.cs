using Shared.Interfaces;

namespace Profile.Domain.Models;

public class User : ISoftDeletable
{
    public string Id { get; set; }
    public string Email { get; set; }
    public DateTime? DeletedAt { get; set; }
    public UserProfile Profile { get; set; }
}