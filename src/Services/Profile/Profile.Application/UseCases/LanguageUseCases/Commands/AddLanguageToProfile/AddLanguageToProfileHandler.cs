using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;

using Profile.Application.Services.Interfaces;

using Profile.Domain.Interfaces;

using Profile.Domain.Specifications.ProfileSpecifications;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public class AddLanguageToProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<AddLanguageToProfileCommand, List<LanguageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "language";
    
    public async Task<List<LanguageResponseDto>> Handle(AddLanguageToProfileCommand request, CancellationToken cancellationToken)
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

        if (isProfileContainsLanguage)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsLanguage);
        }
        
        await _unitOfWork.LanguageRepository.AddLanguageToProfileAsync(profileWithLanguages, language);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.Dto.ProfileId}";
        var mappedLanguages = _mapper.Map<List<LanguageResponseDto>>(profileWithLanguages.Languages);
        await _cacheService.SetAsync(cacheKey, mappedLanguages,
            cancellationToken: cancellationToken);
        
        return mappedLanguages;
    }
}