using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using Shared.Models;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Delete;

public class DeleteProfileHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteProfileCommand, GeneralResponseDto>
{
    public async Task<GeneralResponseDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.Profiles.GetByIdAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        await _unitOfWork.Profiles.DeleteAsync(profile, cancellationToken);
        
        return new GeneralResponseDto();
    }
}