using Profile.Application.DTOs.Country.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class CountryMapping : AutoMapper.Profile
{
    public CountryMapping()
    {
        CreateMap<Country, CountryResponseDto>();
        
    }
}