namespace Profile.Application.DTOs.Interest.Request;

public class AddInterestToProfileDto
{
    public string ProfileId { get; set; }
    public int InterestId { get; set; }
}