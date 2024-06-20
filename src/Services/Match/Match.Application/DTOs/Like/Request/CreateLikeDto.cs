namespace Match.Application.DTOs.Like.Request;

public class CreateLikeDto
{
    public int Id { get; set; }
    public string ProfileId { get; set; }
    public string TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}