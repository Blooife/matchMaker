using Match.Application.DTOs.Profile.Response;

namespace Match.Application.Services.Interfaces;

public interface IProfileGrpcClient
{
    ProfileResponseDto GetProfileInfo(string profileId);
}