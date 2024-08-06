using MediatR;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.DTOs.Interest.Response;

namespace Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;

public sealed record AddInterestToProfileCommand(AddInterestToProfileDto Dto) : IRequest<List<InterestResponseDto>>
{
    
}