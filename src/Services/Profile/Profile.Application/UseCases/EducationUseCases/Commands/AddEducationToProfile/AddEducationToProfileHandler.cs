using AutoMapper;
using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;

public class AddEducationToProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<AddEducationToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddEducationToProfileCommand request, CancellationToken cancellationToken)
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

        if (isContains)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsEducation);
        }

        ProfileEducation userEducation = _mapper.Map<ProfileEducation>(request.Dto);
        
        await _unitOfWork.EducationRepository.AddEducationToProfile(profileWithEducation, userEducation);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}