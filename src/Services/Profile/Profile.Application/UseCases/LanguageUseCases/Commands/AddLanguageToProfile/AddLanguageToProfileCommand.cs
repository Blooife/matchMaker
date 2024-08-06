using MediatR;
using Profile.Application.DTOs.Language.Request;
using Profile.Application.DTOs.Language.Response;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public sealed record AddLanguageToProfileCommand(AddLanguageToProfileDto Dto) : IRequest<List<LanguageResponseDto>>
{
    
}