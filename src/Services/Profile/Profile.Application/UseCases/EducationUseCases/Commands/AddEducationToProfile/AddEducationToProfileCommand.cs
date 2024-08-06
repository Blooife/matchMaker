using MediatR;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;

public sealed record AddEducationToProfileCommand(AddEducationToProfileDto Dto) : IRequest<List<ProfileEducationResponseDto>>
{
    
}