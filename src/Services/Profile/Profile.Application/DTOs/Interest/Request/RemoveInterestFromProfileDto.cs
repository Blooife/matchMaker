namespace Profile.Application.DTOs.Interest.Request;

public class RemoveInterestFromProfileDto
{
    public string ProfileId { get; set; }
    public int InterestId { get; set; }
}