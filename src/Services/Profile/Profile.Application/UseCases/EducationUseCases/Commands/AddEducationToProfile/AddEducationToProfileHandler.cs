using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;

public class AddEducationToProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<AddEducationToProfileCommand, List<ProfileEducationResponseDto>>
{
    private readonly string _cacheKeyPrefix = "education";
    
    public async Task<List<ProfileEducationResponseDto>> Handle(AddEducationToProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithEducation = await _unitOfWork.EducationRepository.GetProfileWithEducationAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithEducation is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.Dto.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException("Education", request.Dto.EducationId);
        }
        
        var isProfileContainsEducation = profileWithEducation.ContainsEducation(request.Dto.EducationId);

        if (isProfileContainsEducation)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsEducation);
        }

        var profileEducation = _mapper.Map<ProfileEducation>(request.Dto);
        await _unitOfWork.EducationRepository.AddEducationToProfileAsync(profileWithEducation, profileEducation);
        await _unitOfWork.SaveAsync(cancellationToken);

        profileEducation.Education = education;
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.Dto.ProfileId}";
        var mappedEducations = _mapper.Map<List<ProfileEducationResponseDto>>(profileWithEducation.ProfileEducations);
        await _cacheService.SetAsync(cacheKey, mappedEducations,
            cancellationToken: cancellationToken);
        
        return mappedEducations;
    }
}