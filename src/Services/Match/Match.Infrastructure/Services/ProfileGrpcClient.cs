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

    public ProfileResponseDto GetProfileInfo(string profileId)
    {
        var request = new GetProfileRequest() { ProfileId = profileId };
        var response = _client.GetProfileById(request);
        var profile = response.Profile;
        return _mapper.Map<ProfileResponseDto>(response);
    }
}