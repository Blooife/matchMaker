using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetById;

public class GetLanguageByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetLanguageByIdQuery, LanguageResponseDto>
{
    private readonly string _cacheKeyPrefix = "language";
    
    public async Task<LanguageResponseDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.LanguageId}";
        var cachedData = await _cacheService.GetAsync<LanguageResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(request.LanguageId, cancellationToken);
        
        if (language == null)
        {
            throw new NotFoundException("Language", request.LanguageId);
        }
        
        var mappedLanguage = _mapper.Map<LanguageResponseDto>(language);
        await _cacheService.SetAsync(cacheKey, mappedLanguage, cancellationToken:cancellationToken);
        
        return mappedLanguage;
    }
}