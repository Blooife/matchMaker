using AutoMapper;
using MediatR;
using Profile.Application.DTOs.User.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Interfaces.Repositories;
using Profile.Domain.Interfaces.Services;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.UserUseCases.Commands.Delete;

public class DeleteUserHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ICacheService _cacheService, ProducerService _producerService) : IRequestHandler<DeleteUserCommand, UserResponseDto>
{
    private readonly string _cacheKeyPrefix = "profile";
    
    public async Task<UserResponseDto> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(request.UserId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User", request.UserId);
        } 
        
        await _unitOfWork.UserRepository.DeleteUserAsync(user);

        var profiles =
            await _unitOfWork.ProfileRepository.GetAsync(profile => profile.UserId == user.Id, cancellationToken);
        var profile = profiles.First();
        await _unitOfWork.ProfileRepository.DeleteProfileAsync(profile);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var cacheKeyProfile = $"{_cacheKeyPrefix}:{profile.Id}";
        await _cacheService.RemoveAsync(cacheKeyProfile, cancellationToken:cancellationToken);
        
        await _producerService.ProduceAsync(new ProfileDeletedMessage(){Id = profile.Id});
        
        return _mapper.Map<UserResponseDto>(user);;
    }
}