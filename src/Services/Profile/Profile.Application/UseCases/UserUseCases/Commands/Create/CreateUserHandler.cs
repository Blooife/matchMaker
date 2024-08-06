using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Services.Interfaces;
using Profile.Application.Exceptions;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;

namespace Profile.Application.UseCases.UserUseCases.Commands.Create;

public class CreateUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<CreateUserCommand, UserResponseDto>
{
    private readonly string _cacheKeyPrefix = "user";
    
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
        
        var cacheKey = $"{_cacheKeyPrefix}:{result.Id}";
        var mappedUser = _mapper.Map<UserResponseDto>(result);
        await _cacheService.SetAsync(cacheKey, mappedUser, cancellationToken:cancellationToken);
        
        return mappedUser;
    }
}