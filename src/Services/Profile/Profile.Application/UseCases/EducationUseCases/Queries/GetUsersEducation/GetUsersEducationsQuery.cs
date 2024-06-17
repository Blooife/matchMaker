using MediatR;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetUsersEducation;

public sealed record GetUsersEducationsQuery(string ProfileId) : IRequest<IEnumerable<EducationResponseDto>>;