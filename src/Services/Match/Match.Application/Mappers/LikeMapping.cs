using Match.Application.DTOs.Like.Request;
using Match.Application.DTOs.Like.Response;
using Match.Domain.Models;

namespace Match.Application.Mappers;

public class LikeMapping : AutoMapper.Profile
{
    public LikeMapping()
    {
        CreateMap<AddLikeDto, Like>()
            .ForMember(dest=>dest.ProfileId, opt=>opt.MapFrom(src=>src.ProfileId))
            .ForMember(dest=>dest.TargetProfileId, opt=>opt.MapFrom(src=>src.TargetProfileId))
            .ForMember(dest=>dest.IsLike, opt=>opt.MapFrom(src=>src.IsLike));
        
        CreateMap<Like, LikeResponseDto>()
            .ForMember(dest=>dest.Id, opt=>opt.MapFrom(src=>src.Id))
            .ForMember(dest=>dest.ProfileId, opt=>opt.MapFrom(src=>src.ProfileId))
            .ForMember(dest=>dest.TargetProfileId, opt=>opt.MapFrom(src=>src.TargetProfileId))
            .ForMember(dest=>dest.IsLike, opt=>opt.MapFrom(src=>src.IsLike));
    }
}