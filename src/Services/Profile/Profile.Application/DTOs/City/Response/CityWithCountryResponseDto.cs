using Profile.Application.DTOs.Country.Response;

namespace Profile.Application.DTOs.City.Response;

public class CityWithCountryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CountryResponseDto Country { get; set; }
}