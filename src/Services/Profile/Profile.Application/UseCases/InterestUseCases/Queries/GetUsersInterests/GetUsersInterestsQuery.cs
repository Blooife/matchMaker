using MediatR;
using Profile.Application.DTOs.Interest.Response;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetUsersInterests;

public sealed record GetUsersInterestsQuery(string ProfileId) : IRequest<IEnumerable<InterestResponseDto>>;