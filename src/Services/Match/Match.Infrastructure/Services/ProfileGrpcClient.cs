using AutoMapper;
using Grpc.Net.Client;
using Match.Application.DTOs.Profile.Response;
using Match.Application.Services.Interfaces;
using Match.Infrastructure.Protos;

namespace Match.Infrastructure.Services;
public class ProfileGrpcClient : IProfileGrpcClient
{
    private readonly ProfileService.ProfileServiceClient _client;
    private readonly IMapper _mapper;

    public ProfileGrpcClient(string grpcServiceUrl, IMapper mapper)
    {
        var channel = GrpcChannel.ForAddress(grpcServiceUrl);
        _client = new ProfileService.ProfileServiceClient(channel);
        _mapper = mapper;
    }

    public async Task<List<FullProfileResponseDto>> GetProfilesInfo(IEnumerable<string> profileIds)
    {
        var request = new GetProfilesRequest();
        request.ProfileIds.AddRange(profileIds);
            
        var response = await _client.GetProfilesByIdsAsync(request);
        
        return _mapper.Map<List<FullProfileResponseDto>>(response.Profiles);
    }
}