using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetUsersLanguages;

public class GetUsersLanguagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUsersLanguagesQuery, IEnumerable<LanguageResponseDto>>
{
    public async Task<IEnumerable<LanguageResponseDto>> Handle(GetUsersLanguagesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetProfileByIdAsync(request.ProfileId, cancellationToken);
        if (profile == null)
        {
            //
        }
        
        var interests = await _unitOfWork.LanguageRepository.GetUsersLanguages(profile, cancellationToken);
        return _mapper.Map<IEnumerable<LanguageResponseDto>>(interests);
    }
}