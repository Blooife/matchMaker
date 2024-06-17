using MediatR;
using Profile.Application.DTOs.Goal.Response;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetById;

public sealed record GetGoalByIdQuery(int GoalId) : IRequest<GoalResponseDto>
{
    
}