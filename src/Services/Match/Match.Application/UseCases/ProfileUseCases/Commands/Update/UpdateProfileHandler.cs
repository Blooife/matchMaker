using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Domain.Repositories;
using MediatR;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var profileMapped = _mapper.Map<Profile>(request.Dto);
        var profile = await profileRepository.GetByIdAsync(profileMapped.Id, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException();
        }
        
        await profileRepository.UpdateAsync(profileMapped, cancellationToken);
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}