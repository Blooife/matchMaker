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
        var alreadyExistsProfile = await profileRepository.GetByIdAsync(request.Dto.Id, cancellationToken);
        
        if (alreadyExistsProfile is not null)
        {
            throw new Exception();
        }
        
        var profile = _mapper.Map<Profile>(request.Dto);
        await profileRepository.CreateAsync(profile, cancellationToken);
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}