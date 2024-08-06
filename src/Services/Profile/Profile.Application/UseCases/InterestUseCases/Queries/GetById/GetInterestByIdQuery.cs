using MediatR;
using Profile.Application.DTOs.Interest.Response;

namespace Profile.Application.UseCases.InterestUseCases.Queries.GetById;

public sealed record GetInterestByIdQuery(int InterestId) : IRequest<InterestResponseDto>;