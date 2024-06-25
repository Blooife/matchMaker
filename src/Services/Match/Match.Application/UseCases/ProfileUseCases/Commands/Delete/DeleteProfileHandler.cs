using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Delete;

public class DeleteProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<DeleteProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var profile = await profileRepository.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile == null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        await profileRepository.DeleteAsync(profile, cancellationToken);
        
        return new GeneralResponseDto();
    }
}