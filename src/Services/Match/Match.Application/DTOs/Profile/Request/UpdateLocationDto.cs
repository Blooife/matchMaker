namespace Match.Application.DTOs.Profile.Request;

public class UpdateLocationDto
{
    public string ProfileId { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}