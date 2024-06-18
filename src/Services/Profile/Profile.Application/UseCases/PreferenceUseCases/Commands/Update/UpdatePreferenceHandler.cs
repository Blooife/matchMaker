using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Preference.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.PreferenceUseCases.Commands.Update;

public class UpdatePreferenceHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdatePreferenceCommand, PreferenceResponseDto>
{
    public async Task<PreferenceResponseDto> Handle(UpdatePreferenceCommand request, CancellationToken cancellationToken)
    {
        var preference =
            await _unitOfWork.PreferenceRepository.GetPreferenceByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (preference is null)
        {
            throw new NotFoundException("Preference", request.Dto.ProfileId);
        }
        
        var mappedPreference = _mapper.Map<Preference>(request.Dto);
        var result = await _unitOfWork.PreferenceRepository.UpdatePreferenceAsync(mappedPreference, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<PreferenceResponseDto>(result);
    }
}