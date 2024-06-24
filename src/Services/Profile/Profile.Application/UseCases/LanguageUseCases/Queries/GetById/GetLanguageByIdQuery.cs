using MediatR;
using Profile.Application.DTOs.Language.Response;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetById;

public sealed record GetLanguageByIdQuery(int LanguageId) : IRequest<LanguageResponseDto>;