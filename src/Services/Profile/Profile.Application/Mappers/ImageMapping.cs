using Profile.Application.DTOs.Image.Response;
using Profile.Domain.Models;

namespace Profile.Application.Mappers;

public class ImageMapping : AutoMapper.Profile
{
    public ImageMapping()
    {
        CreateMap<Image, ImageResponseDto>().ReverseMap();
    }
}