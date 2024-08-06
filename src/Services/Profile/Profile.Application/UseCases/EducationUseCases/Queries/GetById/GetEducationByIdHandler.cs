using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetById;

public class GetEducationByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetEducationByIdQuery, EducationResponseDto>
{
    private readonly string _cacheKeyPrefix = "education";
    
    public async Task<EducationResponseDto> Handle(GetEducationByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}:{request.EducationId}";
        var cachedData = await _cacheService.GetAsync<EducationResponseDto>(cacheKey, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var education = await _unitOfWork.EducationRepository.FirstOrDefaultAsync(request.EducationId, cancellationToken);
        
        if (education is null)
        {
            throw new NotFoundException("Education", request.EducationId);
        }
        
        var mappedEducation = _mapper.Map<EducationResponseDto>(education);
        await _cacheService.SetAsync(cacheKey, mappedEducation, cancellationToken:cancellationToken);
        
        return mappedEducation;
    }
}