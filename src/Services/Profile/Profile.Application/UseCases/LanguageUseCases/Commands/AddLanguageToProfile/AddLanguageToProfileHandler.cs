using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Interfaces;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public class AddLanguageToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddLanguageToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddLanguageToProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithLanguages = await _unitOfWork.LanguageRepository.GetProfileWithLanguages(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithLanguages is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var language = await _unitOfWork.LanguageRepository.FirstOrDefaultAsync(request.Dto.LanguageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException("Language", request.Dto.LanguageId);
        }

        var isContains = profileWithLanguages.ContainsLanguage(request.Dto.LanguageId);

        if (isContains)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsLanguage);
        }
        
        await _unitOfWork.LanguageRepository.AddLanguageToProfile(profileWithLanguages, language);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}