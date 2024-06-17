using MediatR;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetAll;

public sealed record GetAllEducationsQuery : IRequest<IEnumerable<EducationResponseDto>>
{
    
}