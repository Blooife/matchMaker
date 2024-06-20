using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Domain.Repositories;
using MediatR;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profileRepository = _unitOfWork.Profiles;
        var profile = _mapper.Map<Profile>(request.Dto);
        await profileRepository.CreateAsync(profile, cancellationToken);
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}