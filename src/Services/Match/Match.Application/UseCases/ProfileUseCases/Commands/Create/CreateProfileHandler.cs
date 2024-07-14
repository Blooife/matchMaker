using AutoMapper;
using Match.Application.DTOs.Profile.Response;
using Match.Application.Exceptions;
using Match.Domain.Interfaces;
using MediatR;
using Profile = Match.Domain.Models.Profile;

namespace Match.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var existingProfile = await _unitOfWork.Profiles.GetByIdAsync(request.Dto.Id, cancellationToken);
        
        if (existingProfile is not null)
        {
            throw new AlreadyExistsException("Profile already exists in database");
        }
        
        var profile = _mapper.Map<Profile>(request.Dto);
        await _unitOfWork.Profiles.CreateAsync(profile, cancellationToken);
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}