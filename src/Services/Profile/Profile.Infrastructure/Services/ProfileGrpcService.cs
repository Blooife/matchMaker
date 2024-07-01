using Grpc.Core;
using Profile.Infrastructure.Protos;
using Profile.Domain.Repositories;

namespace Profile.Infrastructure.Services;

public class ProfileGrpcService(IUnitOfWork _unitOfWork) : ProfileService.ProfileServiceBase
{
    public override async Task<GetProfilesResponse> GetProfilesByIds(GetProfilesRequest request, ServerCallContext context)
    {
        var profiles = await _unitOfWork.ProfileRepository.GetAllProfileInfoByIdsAsync(request.ProfileIds);

        var response = new GetProfilesResponse();
        
        response.Profiles.AddRange(profiles.Select(profile =>
            new Protos.Profile
            {
                Id = profile.Id,
                Name = profile.Name,
                LastName = profile.LastName,
                BirthDate = profile.BirthDate.ToString("o"),
                Gender = (Gender)profile.Gender,
                Bio = profile.Bio ?? "",
                Height = profile.Height ?? 0,
                ShowAge = profile.ShowAge,
                AgeFrom = profile.AgeFrom,
                AgeTo = profile.AgeTo,
                MaxDistance = profile.MaxDistance,
                PreferredGender = (Gender)profile.PreferredGender,
                Goal = profile.Goal != null ? new Goal { Id = profile.Goal.Id, Name = profile.Goal.Name } : null,
                City = new City
                {
                    Id = profile.City.Id,
                    Name = profile.City.Name,
                    Country = new Country
                    {
                        Id = profile.City.Country.Id,
                        Name = profile.City.Country.Name
                    }
                },
                Languages =
                {
                    profile.Languages.Select(lang => new Language { Id = lang.Id, Name = lang.Name })
                },
                Interests =
                {
                    profile.Interests.Select(interest => new Interest { Id = interest.Id, Name = interest.Name })
                },
                Images =
                {
                    profile.Images.Select(img => new Image { Id = img.Id, Url = img.ImageUrl })
                }
        }));

        return response;
    }
}