namespace Profile.Domain.Models;

public class Image : BaseModel<int>
{
    public string ImageUrl { get; set; }
    public string ProfileId { get; set; }
    public bool IsMainImage { get; set; }
    public DateTime UploadTimestamp { get; set; }
    public UserProfile Profile { get; set; }
}