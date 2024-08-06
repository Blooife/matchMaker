using MediatR;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetProfilesEducation;

public sealed record GetProfilesEducationsQuery(string ProfileId) : IRequest<IEnumerable<ProfileEducationResponseDto>>;