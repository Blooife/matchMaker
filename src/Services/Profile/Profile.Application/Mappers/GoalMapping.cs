using Profile.Application.DTOs.Goal.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class GoalMapping : AutoMapper.Profile
{
    public GoalMapping()
    {
        CreateMap<Goal, GoalResponseDto>().ReverseMap();
    }
}