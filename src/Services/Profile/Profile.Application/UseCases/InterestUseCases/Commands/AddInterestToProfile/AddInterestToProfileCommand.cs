using MediatR;
using Profile.Application.DTOs.Interest.Request;
using Shared.Models;

namespace Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;

public sealed record AddInterestToProfileCommand(AddInterestToProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}