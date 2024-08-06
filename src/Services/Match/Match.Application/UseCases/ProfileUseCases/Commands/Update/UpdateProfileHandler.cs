using AutoMapper;
using Match.Application.Exceptions;
using Match.Domain.Interfaces;
using MediatR;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdateProfileCommand>
{
    public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profileMapped = _mapper.Map<Profile>(request.Dto);
        var profile = await _unitOfWork.Profiles.GetByIdAsync(profileMapped.Id, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.Dto.Id);
        }
        
        await _unitOfWork.Profiles.UpdateAsync(profileMapped, cancellationToken);
    }
}