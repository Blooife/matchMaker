using MediatR;
using Profile.Application.DTOs.Goal.Response;

namespace Profile.Application.UseCases.GoalUseCases.Queries.GetAll;

public sealed record GetAllGoalsQuery : IRequest<IEnumerable<GoalResponseDto>>
{
    
}