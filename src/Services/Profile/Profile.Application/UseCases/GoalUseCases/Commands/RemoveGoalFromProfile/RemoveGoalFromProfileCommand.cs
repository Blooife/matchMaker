using MediatR;
using Profile.Application.DTOs.Goal.Request;
using Shared.Models;

namespace Profile.Application.UseCases.GoalUseCases.Commands.RemoveGoalFromProfile;

public sealed record RemoveGoalFromProfileCommand(RemoveGoalFromProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}