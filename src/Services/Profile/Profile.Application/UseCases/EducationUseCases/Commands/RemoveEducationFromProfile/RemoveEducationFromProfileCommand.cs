using MediatR;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;

public sealed record RemoveEducationFromProfileCommand(RemoveEducationFromProfileDto Dto) : IRequest<List<ProfileEducationResponseDto>>
{
    
}