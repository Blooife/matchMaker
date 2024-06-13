using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Preference.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.PreferenceUseCases.Queries.GetById;

public class GetPreferenceByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPreferenceByIdQuery, PreferenceResponseDto>
{
    public async Task<PreferenceResponseDto> Handle(GetPreferenceByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.PreferenceRepository.GetPreferenceByIdAsync(request.Id, cancellationToken);
        if (result == null)
        {
            //to do exc
        }
        return _mapper.Map<PreferenceResponseDto>(result);
    }
}