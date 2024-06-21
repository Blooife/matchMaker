using Profile.Domain.Models;

namespace Profile.Domain.Models;

public class Image
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public string ProfileId { get; set; }
    
    public UserProfile Profile { get; set; }
}