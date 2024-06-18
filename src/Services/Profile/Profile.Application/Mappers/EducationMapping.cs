using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class EducationMapping : AutoMapper.Profile
{
    public EducationMapping()
    {
        CreateMap<Education, EducationResponseDto>();
        
        CreateMap<UserEducation, UserEducationResponseDto>()
            .ForMember(dest=>dest.Description, opt => opt.MapFrom(src=>src.Description))
            .ForMember(dest=>dest.ProfileId, opt => opt.MapFrom(src=>src.ProfileId))
            .ForMember(dest=>dest.EducationId, opt => opt.MapFrom(src=>src.EducationId))
            .ForMember(dest=>dest.EducationName, opt => opt.MapFrom(src=>src.Education.Name));
    }
}