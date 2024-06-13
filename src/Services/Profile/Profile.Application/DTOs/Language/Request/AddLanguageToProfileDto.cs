namespace Profile.Application.DTOs.Language.Request;

public class AddLanguageToProfileDto
{
    public string ProfileId { get; set; }
    public int LanguageId { get; set; }
}