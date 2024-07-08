using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Services.Interfaces;
using Profile.Domain.Repositories;

namespace Profile.Application.UseCases.UserUseCases.Commands.Delete;

public class DeleteUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService) : IRequestHandler<DeleteUserCommand, UserResponseDto>
{
    private readonly string _cacheKeyPrefix = "user";
    
    public async Task<UserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User", request.UserId);
        }
        
        await _unitOfWork.UserRepository.DeleteUserAsync(user, cancellationToken);

        var profiles =
            await _unitOfWork.ProfileRepository.GetAsync(profile => profile.UserId == user.Id, cancellationToken);
        var profile = profiles.First();
        await _unitOfWork.ProfileRepository.DeleteProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKey = $"{_cacheKeyPrefix}:{user.Id}";
        var mappedUser = _mapper.Map<UserResponseDto>(user);
        await _cacheService.RemoveAsync(cacheKey, cancellationToken:cancellationToken);
        
        var cacheKeyProfile = $"profile:{profile.Id}";
        await _cacheService.RemoveAsync(cacheKeyProfile, cancellationToken:cancellationToken);
        
        return mappedUser;
    }
}