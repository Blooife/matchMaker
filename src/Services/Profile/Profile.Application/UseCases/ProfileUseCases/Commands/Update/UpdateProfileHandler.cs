using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var findRes =
            await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.UpdateProfileDto.Id, cancellationToken);
        
        if (findRes == null)
        {
            //to do exc
        }
        
        var profile = _mapper.Map<UserProfile>(request.UpdateProfileDto);
        var result = await _unitOfWork.ProfileRepository.UpdateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<ProfileResponseDto>(result);
    }
}