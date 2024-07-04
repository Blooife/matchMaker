using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.UserUseCases.Commands.Delete;

public class DeleteUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<DeleteUserCommand, UserResponseDto>
{
    public async Task<UserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User", request.UserId);
        } 
        
        await _unitOfWork.UserRepository.DeleteUserAsync(user, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        return _mapper.Map<UserResponseDto>(user);
    }
}