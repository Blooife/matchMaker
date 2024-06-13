using MediatR;
using Profile.Domain.Repositories;
using Shared.Models;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.RemoveLanguageFromProfile;

public class RemoveLanguageFromProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveLanguageFromProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(RemoveLanguageFromProfileCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.LanguageRepository.RemoveLanguageFromProfile(request.Dto.ProfileId, request.Dto.LanguageId, cancellationToken);
        return new GeneralResponseDto();
    }
}