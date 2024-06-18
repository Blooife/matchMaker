using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Language.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.LanguageUseCases.Queries.GetUsersLanguages;

public class GetUsersLanguagesHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUsersLanguagesQuery, IEnumerable<LanguageResponseDto>>
{
    public async Task<IEnumerable<LanguageResponseDto>> Handle(GetUsersLanguagesQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        var interests = await _unitOfWork.LanguageRepository.GetUsersLanguages(profile, cancellationToken);
        
        return _mapper.Map<IEnumerable<LanguageResponseDto>>(interests);
    }
}