using AutoMapper;
using MediatR;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;

public class RemoveEducationFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveEducationFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveEducationFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithEducation = await _unitOfWork.EducationRepository.GetUserWithEducation(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithEducation is null)
        {
            throw new Exception();
        }
        
        var education = await _unitOfWork.EducationRepository.GetByIdAsync(request.Dto.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new Exception();
        }
        
        var isContains = profileWithEducation.ContainsEducation(request.Dto.EducationId);

        if (isContains)
        {
            throw new Exception();
        }

        UserEducation userEducation = profileWithEducation.UserEducations.First(userEducation=>userEducation.EducationId == request.Dto.EducationId);
        
        await _unitOfWork.EducationRepository.RemoveEducationFromProfile(profileWithEducation, userEducation);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}