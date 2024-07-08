using MediatR;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.DTOs.Language.Response;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public sealed record RemoveLanguageFromProfileCommand(RemoveLanguageFromProfileDto Dto) : IRequest<List<LanguageResponseDto>>
{
    
}