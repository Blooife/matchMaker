using MediatR;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public class RemoveLanguageFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveLanguageFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveLanguageFromProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithInterests = await _unitOfWork.InterestRepository.GetUserWithInterests(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithInterests is null)
        {
            throw new Exception();
        }
        
        var language = await _unitOfWork.InterestRepository.GetByIdAsync(request.Dto.LanguageId, cancellationToken);
        
        if (language is null)
        {
            throw new Exception();
        }

        var isContains = profileWithInterests.ContainsLanguage(request.Dto.LanguageId);

        if (!isContains)
        {
            throw new Exception();
        }
        
        await _unitOfWork.InterestRepository.RemoveInterestFromProfile(profileWithInterests, language);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}