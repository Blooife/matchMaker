namespace Profile.Application.DTOs.City.Response;

public class CityWithCountryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
}