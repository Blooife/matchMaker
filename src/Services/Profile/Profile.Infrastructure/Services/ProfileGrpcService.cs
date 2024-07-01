using Grpc.Core;
using Profile.Domain.Repositories;
using Profile.Infrastructure.Protos;

namespace Profile.Infrastructure.Services;

public class ProfileGrpcService(IUnitOfWork _unitOfWork) : ProfileService.ProfileServiceBase
{
    public override async Task<GetProfileResponse> GetProfileById(GetProfileRequest request, ServerCallContext context)
    {
        var userProfile = await _unitOfWork.ProfileRepository.GetAllProfileInfoByIdAsync(request.ProfileId);

        if (userProfile == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Profile not found"));
        }

        var response = new GetProfileResponse
        {
            Profile = new Protos.Profile()
            {
                Id = userProfile.Id,
                Name = userProfile.Name,
                LastName = userProfile.LastName,
                BirthDate = userProfile.BirthDate.ToString("o"),
                Gender = (Gender)userProfile.Gender,
                Bio = userProfile.Bio,
                Height = userProfile.Height ?? 0,
                ShowAge = userProfile.ShowAge,
                AgeFrom = userProfile.AgeFrom,
                AgeTo = userProfile.AgeTo,
                MaxDistance = userProfile.MaxDistance,
                PreferredGender = (Gender)userProfile.PreferredGender,
                UserId = userProfile.UserId,
                Goal = userProfile.Goal != null ? new Goal { Id = userProfile.Goal.Id, Name = userProfile.Goal.Name } : null,
                City = new City
                {
                    Id = userProfile.City.Id,
                    Name = userProfile.City.Name,
                    Country = new Country
                    {
                        Id = userProfile.City.Country.Id,
                        Name = userProfile.City.Country.Name
                    }
                },
                Languages = { userProfile.Languages.Select(lang => new Language { Id = lang.Id, Name = lang.Name }) },
                Interests = { userProfile.Interests.Select(interest => new Interest { Id = interest.Id, Name = interest.Name }) },
                Educations = { userProfile.ProfileEducations.Select(pe => new ProfileEducation
                {
                    ProfileId = pe.ProfileId,
                    Education = new Education { Id = pe.Education.Id, Name = pe.Education.Name },
                    Description = pe.Description
                }) },
                Images = { userProfile.Images.Select(img => new Image { Id = img.Id, Url = img.ImageUrl }) }
            }
        };

        return response;
    }
}