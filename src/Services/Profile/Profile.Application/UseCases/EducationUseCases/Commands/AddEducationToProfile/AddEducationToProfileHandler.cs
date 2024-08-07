using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;

public class AddEducationToProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<AddEducationToProfileCommand, List<ProfileEducationResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<List<ProfileEducationResponseDto>> Handle(AddEducationToProfileCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.Dto.ProfileId}";
        var profileResponseDto = await _cacheService.GetAsync(cacheKey, async () =>
        {
            var profile = await _unitOfWork.ProfileRepository.GetAllProfileInfoAsync(userProfile => userProfile.Id == request.Dto.ProfileId, cancellationToken);
            
            return _mapper.Map<ProfileResponseDto>(profile);
        }, cancellationToken);

        var profile = _mapper.Map<UserProfile>(profileResponseDto);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.Dto.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException("Education", request.Dto.EducationId);
        }
        
        var isProfileContainsEducation = profile.ContainsEducation(request.Dto.EducationId);

        if (isProfileContainsEducation)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsEducation);
        }

        var profileEducation = _mapper.Map<ProfileEducation>(request.Dto);
        await _unitOfWork.EducationRepository.AddEducationToProfileAsync(profile, profileEducation);
        await _unitOfWork.SaveAsync(cancellationToken);

        profileEducation.Education = education;
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return _mapper.Map<List<ProfileEducationResponseDto>>(profile.ProfileEducations);
    }
}