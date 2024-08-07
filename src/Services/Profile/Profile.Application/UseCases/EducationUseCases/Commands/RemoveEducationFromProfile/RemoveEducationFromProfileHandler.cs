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

namespace Profile.Application.UseCases.EducationUseCases.Commands.RemoveEducationFromProfile;

public class RemoveEducationFromProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<RemoveEducationFromProfileCommand, List<ProfileEducationResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<List<ProfileEducationResponseDto>> Handle(RemoveEducationFromProfileCommand request, CancellationToken cancellationToken)
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

        if (!isProfileContainsEducation)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsEducation);
        }

        ProfileEducation userEducation = profile.ProfileEducations.First(userEducation=>userEducation.EducationId == request.Dto.EducationId);
        
        await _unitOfWork.EducationRepository.RemoveEducationFromProfileAsync(profile, userEducation);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return _mapper.Map<List<ProfileEducationResponseDto>>(profile.ProfileEducations);
    }
}