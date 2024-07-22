using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public class RemoveLanguageFromProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<RemoveLanguageFromProfileCommand, List<LanguageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "language";
    
    public async Task<List<LanguageResponseDto>> Handle(RemoveLanguageFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithLanguages = await _unitOfWork.LanguageRepository.GetProfileWithLanguagesAsync(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithLanguages is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(request.Dto.LanguageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException("Language", request.Dto.LanguageId);
        }

        var isProfileContainsLanguage = profileWithLanguages.ContainsLanguage(request.Dto.LanguageId);

        if (!isProfileContainsLanguage)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsLanguage);
        }

        var languageToRemove = profileWithLanguages.Languages.First(l=>l.Id == request.Dto.LanguageId);
        await _unitOfWork.LanguageRepository.RemoveLanguageFromProfileAsync(profileWithLanguages, languageToRemove, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.Dto.ProfileId}";
        var mappedLanguages = _mapper.Map<List<LanguageResponseDto>>(profileWithLanguages.Languages);
        await _cacheService.SetAsync(cacheKey, mappedLanguages,
            cancellationToken: cancellationToken);
        
        return mappedLanguages;
    }
}