using MediatR;
using Profile.Application.DTOs.Language.Response;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetAll;

public sealed record GetAllLanguagesQuery : IRequest<IEnumerable<LanguageResponseDto>>
{
    
}