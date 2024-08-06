using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetAll;

public class GetAllILanguagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetAllLanguagesQuery, IEnumerable<LanguageResponseDto>>
{
    public async Task<IEnumerable<LanguageResponseDto>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        var languages = await _unitOfWork.LanguageRepository.GetAllAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<LanguageResponseDto>>(languages);
    }
}