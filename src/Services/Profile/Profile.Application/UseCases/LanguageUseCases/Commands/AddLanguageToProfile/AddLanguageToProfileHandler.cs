using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Profile.Domain.Models;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public class AddLanguageToProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<AddLanguageToProfileCommand, List<LanguageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<List<LanguageResponseDto>> Handle(AddLanguageToProfileCommand request, CancellationToken cancellationToken)
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

        if (isProfileContainsLanguage)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsLanguage);
        }
        
        await _unitOfWork.LanguageRepository.AddLanguageToProfileAsync(profile, language);
        
        await _unitOfWork.SaveAsync(cancellationToken);
        
        await _cacheService.SetAsync(cacheKey, _mapper.Map<ProfileResponseDto>(profile),
            cancellationToken: cancellationToken);
        
        return _mapper.Map<List<LanguageResponseDto>>(profile.Languages);
    }
}