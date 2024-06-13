using MediatR;
using Profile.Application.DTOs.Interest.Request;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.RemoveInterestFromProfile;

public sealed record RemoveInterestFromProfileCommand(RemoveInterestFromProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}