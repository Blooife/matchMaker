namespace Match.Application.DTOs.Like.Response;

public class LikeResponseDto
{
    public string Id { get; set; }
    public string ProfileId { get; set; }
    public string TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}