using Match.Application.DTOs.Profile.Response;

namespace Match.Application.Services.Interfaces;

public interface IProfileGrpcClient
{
    Task<List<FullProfileResponseDto>> GetProfilesInfo(IEnumerable<string> profileIds);
}