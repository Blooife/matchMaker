using AutoMapper;
using MediatR;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public class AddLanguageToProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddLanguageToProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(AddLanguageToProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.GetProfileByIdAsync(request.Dto.ProfileId, cancellationToken);
        if (profile == null)
        {
            //to do exc
        }

        var language = await _unitOfWork.LanguageRepository.GetByIdAsync(request.Dto.LanguageId, cancellationToken);
        if (language == null)
        {
            //to do exc
        }

        var languages = await _unitOfWork.LanguageRepository.GetUsersLanguages(profile, cancellationToken);
        
        await _unitOfWork.LanguageRepository.AddLanguageToProfile(profile, language, cancellationToken);
        
        return new GeneralResponseDto();
    }
}