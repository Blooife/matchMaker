using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;

public class RemoveEducationFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveEducationFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveEducationFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithEducation = await _unitOfWork.EducationRepository.GetProfileWithEducation(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithEducation is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.Dto.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException("Education", request.Dto.EducationId);
        }
        
        var isContains = profileWithEducation.ContainsEducation(request.Dto.EducationId);

        if (!isContains)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsEducation);
        }

        ProfileEducation userEducation = profileWithEducation.ProfileEducations.First(userEducation=>userEducation.EducationId == request.Dto.EducationId);
        
        await _unitOfWork.EducationRepository.RemoveEducationFromProfile(profileWithEducation, userEducation);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}