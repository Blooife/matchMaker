using MediatR;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.PreferenceUseCases.Commands.ChangeIsActive;

public class ChangeIsActiveHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeIsActiveCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(ChangeIsActiveCommand request, CancellationToken cancellationToken)
    {
        var preference =
            await _unitOfWork.PreferenceRepository.GetPreferenceByIdAsync(request.ProfileId, cancellationToken);
        
        if (preference is null)
        {
            throw new NotFoundException("Preference", request.ProfileId);
        }
        
        await _unitOfWork.PreferenceRepository.ChangeIsActive(preference, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}