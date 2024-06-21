using MediatR;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.GoalUseCases.Commands.RemoveGoalFromProfile;

public class RemoveGoalFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveGoalFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveGoalFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        await _unitOfWork.GoalRepository.RemoveGoalFromProfile(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}