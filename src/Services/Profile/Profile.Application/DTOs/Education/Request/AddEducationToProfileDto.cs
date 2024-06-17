namespace Profile.Application.DTOs.Education.Request;

public class AddEducationToProfileDto
{
    public string ProfileId { get; set; }
    public int EducationId { get; set; }
    public string Description { get; set; }
}