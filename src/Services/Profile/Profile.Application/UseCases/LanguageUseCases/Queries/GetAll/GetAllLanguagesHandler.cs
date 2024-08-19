using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetAll;

public class GetAllLanguagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllLanguagesQuery, IEnumerable<LanguageResponseDto>>
{
    private readonly string _cacheKeyPrefix = "languages";
    
    public async Task<IEnumerable<LanguageResponseDto>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<LanguageResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var languages = await _unitOfWork.LanguageRepository.GetAllAsync(cancellationToken);
        
        var mappedLanguages = _mapper.Map<List<LanguageResponseDto>>(languages);
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedLanguages, cancellationToken:cancellationToken);
        
        return mappedLanguages;
    }
}