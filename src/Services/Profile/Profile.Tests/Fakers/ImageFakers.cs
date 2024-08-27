using Bogus;
using Profile.Application.DTOs.Image.Response;
using Profile.Domain.Models;

namespace Profile.Tests.Fakers;

public static class ImageFakers
{
    public static Faker<ImageResponseDto> CreateImageResponseDto()
    {
        return new Faker<ImageResponseDto>()
            .RuleFor(i => i.Id, f => f.Random.Int())
            .RuleFor(i => i.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(i => i.ImageUrl, f => f.Internet.Url()+"/image.jpg")
            .RuleFor(i => i.IsMainImage, false);
    }
    
    public static Faker<Image> CreateImage()
    {
        return new Faker<Image>()
            .RuleFor(i => i.Id, f => f.Random.Int())
            .RuleFor(i => i.ProfileId, f => f.Random.Guid().ToString())
            .RuleFor(i => i.ImageUrl, f => f.Internet.Url())
            .RuleFor(i => i.IsMainImage, false);
    }
}