using MediatR;
using Profile.Application.Exceptions;
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
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var goal = await _unitOfWork.GoalRepository.GetByIdAsync(request.Dto.GoalId, cancellationToken);
        
        if (goal is null)
        {
            throw new NotFoundException("Goal", request.Dto.GoalId);
        }
        
        await _unitOfWork.GoalRepository.AddGoalToProfile(profile, goal, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}