using MediatR;
using Profile.Application.DTOs.Education.Request;
using Profile.Application.DTOs.Education.Response;

namespace Profile.Application.UseCases.EducationUseCases.Commands.Update;

public sealed record UpdateProfileEducationCommand(UpdateProfileEducationDto Dto) : IRequest<ProfileEducationResponseDto>
{
    
}