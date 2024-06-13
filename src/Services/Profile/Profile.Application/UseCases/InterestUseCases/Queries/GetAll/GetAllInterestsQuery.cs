using MediatR;
using Profile.Application.DTOs.Interest.Response;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetAll;

public sealed record GetAllInterestsQuery : IRequest<IEnumerable<InterestResponseDto>>
{
    
}