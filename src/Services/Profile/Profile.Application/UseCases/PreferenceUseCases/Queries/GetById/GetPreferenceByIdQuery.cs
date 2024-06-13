using MediatR;
using Profile.Application.DTOs.Preference.Response;

namespace Profile.Application.UseCases.PreferenceUseCases.Queries.GetById;

public sealed record GetPreferenceByIdQuery(string Id) : IRequest<PreferenceResponseDto>
{
    
}