using Match.Application.DTOs.Like.Request;
using Match.Application.DTOs.Like.Response;
using Match.Domain.Models;

namespace Match.Application.Mappers;

public class LikeMapping : AutoMapper.Profile
{
    public LikeMapping()
    {
        CreateMap<CreateLikeDto, Like>();
        CreateMap<Like, LikeResponseDto>();
    }
}