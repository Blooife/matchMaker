using MediatR;
using Profile.Application.DTOs.Education.Request;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;

public sealed record RemoveEducationFromProfileCommand(RemoveEducationFromProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}