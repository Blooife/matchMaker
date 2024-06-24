using Profile.Application.DTOs.Interest.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class InterestMapping : AutoMapper.Profile
{
    public InterestMapping()
    {
        CreateMap<Interest, InterestResponseDto>();
    }
}