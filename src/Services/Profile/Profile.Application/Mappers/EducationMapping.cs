using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class EducationMapping : AutoMapper.Profile
{
    public EducationMapping()
    {
        CreateMap<Education, EducationResponseDto>();
    }
}