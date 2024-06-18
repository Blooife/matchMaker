namespace Profile.Application.DTOs.Education.Request;

public class UpdateUserEducationDto
{
    public string ProfileId { get; set; }
    public int EducationId { get; set; }
    public string Description { get; set; }
}