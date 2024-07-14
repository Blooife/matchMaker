using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.UserUseCases.Queries.GetById;

public class GetUserByIdHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<GetUserByIdQuery, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User", request.UserId);
        }
        
        return _mapper.Map<UserResponseDto>(user);
    }
}