using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Models;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.UserUseCases.Commands.Update;

public class UpdateUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<UpdateUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var findRes =
            await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.UpdateUserDto.Id, cancellationToken);
        
        if (findRes is null)
        {
            throw new NotFoundException("User", request.UpdateUserDto.Id);
        }
        
        var user = _mapper.Map<User>(request.UpdateUserDto);
        var result = await _unitOfWork.UserRepository.UpdateUserAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<UserResponseDto>(result);
    }
}