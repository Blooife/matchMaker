using MediatR;
using Profile.Application.DTOs.Language.Request;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public sealed record RemoveLanguageFromProfileCommand(RemoveLanguageFromProfileDto Dto) : IRequest<GeneralResponseDto>
{
    
}