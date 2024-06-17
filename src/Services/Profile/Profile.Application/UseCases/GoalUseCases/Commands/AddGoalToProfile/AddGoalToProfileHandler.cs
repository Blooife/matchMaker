using MediatR;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.GoalUseCases.Commands.AddGoalToProfile;

public class AddGoalToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddGoalToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddGoalToProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new Exception();
        }
        
        var goal = await _unitOfWork.GoalRepository.GetByIdAsync(request.Dto.GoalId, cancellationToken);
        
        if (goal is null)
        {
            throw new Exception();
        }
        
        await _unitOfWork.GoalRepository.AddGoalToProfile(profile, goal, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}