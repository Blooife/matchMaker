using MediatR;
using Profile.Application.DTOs.Language.Request;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public sealed record AddLanguageToProfileCommand(AddLanguageToProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}