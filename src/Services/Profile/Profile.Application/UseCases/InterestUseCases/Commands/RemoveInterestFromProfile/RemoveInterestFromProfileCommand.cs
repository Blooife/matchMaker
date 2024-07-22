using MediatR;
using Profile.Application.DTOs.Interest.Request;
using Profile.Application.DTOs.Interest.Response;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public sealed record RemoveInterestFromProfileCommand(RemoveInterestFromProfileDto Dto) : IRequest<List<InterestResponseDto>>
{
    
}