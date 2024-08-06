using MediatR;
using Profile.Application.DTOs.Interest.Response;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetProfilesInterests;

public sealed record GetProfilesInterestsQuery(string ProfileId) : IRequest<IEnumerable<InterestResponseDto>>;