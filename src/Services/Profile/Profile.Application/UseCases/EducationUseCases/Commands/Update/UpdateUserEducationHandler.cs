using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.Update;

public class UpdateUserEducationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateUserEducationCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(UpdateUserEducationCommand request, CancellationToken cancellationToken)
    {
        var profileWithEducation = await _unitOfWork.EducationRepository.GetUserWithEducation(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithEducation is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.GetByIdAsync(request.Dto.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException("Education", request.Dto.EducationId);
        }
        
        var isContains = profileWithEducation.ContainsEducation(request.Dto.EducationId);

        if (!isContains)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsEducation);
        }

        UserEducation userEducation = profileWithEducation.UserEducations.First(userEducation=>userEducation.EducationId == request.Dto.EducationId);
        
        await _unitOfWork.EducationRepository.UpdateUsersEducation(userEducation, request.Dto.Description);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}