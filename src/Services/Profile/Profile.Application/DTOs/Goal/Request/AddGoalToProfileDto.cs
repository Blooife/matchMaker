namespace Profile.Application.DTOs.Goal.Request;

public class AddGoalToProfileDto
{
    public string ProfileId { get; set; }
    public int GoalId { get; set; }
}