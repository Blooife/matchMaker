using MediatR;
using Profile.Application.DTOs.Language.Response;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetUsersLanguages;

public sealed record GetUsersLanguagesQuery(string ProfileId) : IRequest<IEnumerable<LanguageResponseDto>>;