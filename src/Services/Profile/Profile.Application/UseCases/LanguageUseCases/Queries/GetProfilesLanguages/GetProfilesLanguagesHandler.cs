using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetProfilesLanguages;

public class GetProfilesLanguagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfilesLanguagesQuery, IEnumerable<LanguageResponseDto>>
{
    public async Task<IEnumerable<LanguageResponseDto>> Handle(GetProfilesLanguagesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var languages = await _unitOfWork.LanguageRepository.GetProfilesLanguages(request.ProfileId, cancellationToken);
        
        return _mapper.Map<IEnumerable<LanguageResponseDto>>(languages);
    }
}