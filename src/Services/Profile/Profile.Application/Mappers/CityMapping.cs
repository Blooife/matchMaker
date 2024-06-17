using Profile.Application.DTOs.City.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class CityMapping : AutoMapper.Profile
{
    public CityMapping()
    {
        CreateMap<City, CityResponseDto>();
    }
}