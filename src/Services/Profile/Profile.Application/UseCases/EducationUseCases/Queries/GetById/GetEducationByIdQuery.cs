using MediatR;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetById;

public sealed record GetEducationByIdQuery(int EducationId) : IRequest<EducationResponseDto>;