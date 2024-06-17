using MediatR;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.PreferenceUseCases.Commands.ChangeIsActive;

public class ChangeIsActiveHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ChangeIsActiveCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(ChangeIsActiveCommand request, CancellationToken cancellationToken)
    {
        var findRes =
            await _unitOfWork.PreferenceRepository.GetPreferenceByIdAsync(request.ProfileId, cancellationToken);
        
        if (findRes == null)
        {
            //to do exc
        }
        
        await _unitOfWork.PreferenceRepository.ChangeIsActive(findRes, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}