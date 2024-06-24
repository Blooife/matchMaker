using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.UserUseCases.Commands.Create;

public class CreateUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.CreateUserDto);
        var result = await _unitOfWork.UserRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<UserResponseDto>(result);
    }
}