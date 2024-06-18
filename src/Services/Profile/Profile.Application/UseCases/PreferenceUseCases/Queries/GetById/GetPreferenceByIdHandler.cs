using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Preference.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.PreferenceUseCases.Queries.GetById;

public class GetPreferenceByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetPreferenceByIdQuery, PreferenceResponseDto>
{
    public async Task<PreferenceResponseDto> Handle(GetPreferenceByIdQuery request, CancellationToken cancellationToken)
    {
        var preference = await _unitOfWork.PreferenceRepository.GetPreferenceByIdAsync(request.Id, cancellationToken);
        
        if (preference is null)
        {
            throw new NotFoundException("Preference", request.Id);
        }
        
        return _mapper.Map<PreferenceResponseDto>(preference);
    }
}