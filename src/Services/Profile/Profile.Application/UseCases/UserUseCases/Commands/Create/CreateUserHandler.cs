using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.UserUseCases.Commands.Create;

public class CreateUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(request.CreateUserDto.Id, cancellationToken);

        if (existingUser is not null)
        {
            throw new AlreadyExistsException($"User with key = {request.CreateUserDto.Id} already exists");
        }
        
        var user = _mapper.Map<User>(request.CreateUserDto);
        var result = await _unitOfWork.UserRepository.CreateUserAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<UserResponseDto>(result);
    }
}