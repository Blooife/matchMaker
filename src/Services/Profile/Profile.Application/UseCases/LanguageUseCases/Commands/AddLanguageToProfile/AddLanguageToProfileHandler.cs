using AutoMapper;
using MediatR;
using Profile.Application.Exceptions;
using Profile.Application.Exceptions.Messages;
using Profile.Domain.Repositories;
using Profile.Domain.Specifications.ProfileSpecifications;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public class AddLanguageToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddLanguageToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddLanguageToProfileCommand request, CancellationToken cancellationToken)
    {
        var profileWithInterests = await _unitOfWork.InterestRepository.GetUserWithInterests(request.Dto.ProfileId, cancellationToken);
        
        if (profileWithInterests is null)
        {
            throw new NotFoundException("Profile", request.Dto.ProfileId);
        }
        
        var language = await _unitOfWork.InterestRepository.GetByIdAsync(request.Dto.LanguageId, cancellationToken);
        
        if (language is null)
        {
            throw new NotFoundException("Language", request.Dto.LanguageId);
        }

        var isContains = profileWithInterests.ContainsLanguage(request.Dto.LanguageId);

        if (isContains)
        {
            throw new AlreadyContainsException(ExceptionMessages.ProfileContainsLanguage);
        }
        
        await _unitOfWork.InterestRepository.AddInterestToProfile(profileWithInterests, language);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return new GeneralResponseDto();
    }
}