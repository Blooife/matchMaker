using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public class RemoveLanguageFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveLanguageFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveLanguageFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithLanguages = await _unitOfWork.LanguageRepository.GetUserWithLanguages(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithLanguages is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var language = await _unitOfWork.LanguageRepository.GetByIdAsync(request.Dto.LanguageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException("Language", request.Dto.LanguageId);
        }

        var isContains = profileWithLanguages.ContainsLanguage(request.Dto.LanguageId);

        if (!isContains)
        {
            throw new NotContainsException(ExceptionMessages.ProfileNotContainsLanguage);
        }
        
        await _unitOfWork.LanguageRepository.RemoveLanguageFromProfile(profileWithLanguages, language);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}