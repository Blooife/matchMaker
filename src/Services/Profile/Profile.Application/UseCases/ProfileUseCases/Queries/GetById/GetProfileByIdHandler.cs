using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.ProfileUseCases.Queries.GetById;

public class GetProfileByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ProfileRepository.GetProfileByIdAsync(request.ProfileId, cancellationToken);
        if (result == null)
        {
            //to do exception
        }
        return _mapper.Map<ProfileResponseDto>(result);
    }
}