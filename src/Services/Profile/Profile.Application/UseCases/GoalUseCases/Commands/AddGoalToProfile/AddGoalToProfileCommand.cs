using MediatR;
using Profile.Application.DTOs.Goal.Request;
using Shared.Models;

namespace Profile.Application.UseCases.GoalUseCases.Commands.AddGoalToProfile;

public sealed record AddGoalToProfileCommand(AddGoalToProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}