using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetById;

public class GetProfileByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}