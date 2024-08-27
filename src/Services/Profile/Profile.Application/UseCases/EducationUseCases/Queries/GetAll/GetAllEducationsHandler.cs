using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Education.Response;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;

namespace Profile.Application.UseCases.EducationUseCases.Queries.GetAll;

public class GetAllEducationsHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<GetAllEducationsQuery, IEnumerable<EducationResponseDto>>
{
    private readonly string _cacheKeyPrefix = "educations";
    
    public async Task<IEnumerable<EducationResponseDto>> Handle(GetAllEducationsQuery request, CancellationToken cancellationToken)
    {
        var cachedData = await _cacheService.GetAsync<IEnumerable<EducationResponseDto>>(_cacheKeyPrefix, cancellationToken);
        
        if (cachedData is not null)
        {
            return cachedData;
        }
        
        var result = await _unitOfWork.EducationRepository.GetAllAsync(cancellationToken);
        
        var mappedEducations = _mapper.Map<List<EducationResponseDto>>(result);
        
        await _cacheService.SetAsync(_cacheKeyPrefix, mappedEducations, cancellationToken:cancellationToken);
        
        return mappedEducations;
    }
}