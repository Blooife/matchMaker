namespace Match.Application.DTOs.Like.Request;

public class CreateLikeDto
{
    public string ProfileId { get; set; }
    public string TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}