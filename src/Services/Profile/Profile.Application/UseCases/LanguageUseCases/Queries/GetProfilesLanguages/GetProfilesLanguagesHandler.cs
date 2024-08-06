using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetProfilesLanguages;

public class GetProfilesLanguagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetProfilesLanguagesQuery, IEnumerable<LanguageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "language";
    
    public async Task<IEnumerable<LanguageResponseDto>> Handle(GetProfilesLanguagesQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:profile:{request.ProfileId}";
        var cachedData = await _cacheService.GetAsync<List<LanguageResponseDto>>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var languages = await _unitOfWork.LanguageRepository.GetProfilesLanguagesAsync(request.ProfileId, cancellationToken);
        var mappedLanguages = _mapper.Map<List<LanguageResponseDto>>(languages);
        await _cacheService.SetAsync(cacheKey, mappedLanguages, cancellationToken: cancellationToken);
        
        return mappedLanguages;
    }
}