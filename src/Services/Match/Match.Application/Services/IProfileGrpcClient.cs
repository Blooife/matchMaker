using Match.Application.DTOs.Profile.Response;

namespace Match.Application.Services;

public interface IProfileGrpcClient
{
    Task<List<ProfileResponseDto>> GetProfilesInfo(IEnumerable<string> profileIds);
}