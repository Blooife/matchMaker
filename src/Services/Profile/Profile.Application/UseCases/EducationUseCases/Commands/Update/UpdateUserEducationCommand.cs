using MediatR;
using Profile.Application.DTOs.Education.Request;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.Update;

public sealed record UpdateProfileEducationCommand(UpdateProfileEducationDto Dto) : IRequest<GeneralResponseDto>
{
    
}