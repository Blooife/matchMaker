using Profile.Application.DTOs.City.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class CityMapping : AutoMapper.Profile
{
    public CityMapping()
    {
        CreateMap<City, CityResponseDto>();
        
        CreateMap<City, CityWithCountryResponseDto>()
            .ForMember(dest=>dest.CountryName, opt => opt.MapFrom(src=>src.Country.Name))
            .ForMember(dest=>dest.CountryId, opt => opt.MapFrom(src=>src.CountryId));;
    }
}