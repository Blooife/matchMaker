using MediatR;
using Profile.Application.DTOs.Preference.Request;
using Profile.Application.DTOs.Preference.Response;

namespace Profile.Application.UseCases.PreferenceUseCases.Commands.Update;

public sealed record UpdatePreferenceCommand(UpdatePreferenceDto Dto) : IRequest<PreferenceResponseDto>
{
    
}