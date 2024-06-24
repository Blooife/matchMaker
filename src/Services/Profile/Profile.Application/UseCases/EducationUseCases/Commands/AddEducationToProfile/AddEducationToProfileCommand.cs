using MediatR;
using Profile.Application.DTOs.Education.Request;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;

public sealed record AddEducationToProfileCommand(AddEducationToProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}