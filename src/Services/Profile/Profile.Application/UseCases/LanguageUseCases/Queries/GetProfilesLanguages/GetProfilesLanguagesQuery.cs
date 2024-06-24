using MediatR;
using Profile.Application.DTOs.Language.Response;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetProfilesLanguages;

public sealed record GetProfilesLanguagesQuery(string ProfileId) : IRequest<IEnumerable<LanguageResponseDto>>;