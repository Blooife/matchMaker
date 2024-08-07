using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;

using Profile.Domain.Interfaces;
using Profile.Domain.Models;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public class RemoveLanguageFromProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<RemoveLanguageFromProfileCommand, List<LanguageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<List<LanguageResponseDto>> Handle(RemoveLanguageFromProfileCommand request, CancellationToken cancellationToken)
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
        
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(request.Dto.LanguageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException("Language", request.Dto.LanguageId);
        }

        var isProfileContainsLanguage = profile.ContainsLanguage(request.Dto.LanguageId);

        if (!isProfileContainsLanguage)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsLanguage);
        }

        var languageToRemove = profile.Languages.First(l=>l.Id == request.Dto.LanguageId);
        await _unitOfWork.LanguageRepository.RemoveLanguageFromProfileAsync(profile, languageToRemove, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return _mapper.Map<List<LanguageResponseDto>>(profile.Languages);
    }
}