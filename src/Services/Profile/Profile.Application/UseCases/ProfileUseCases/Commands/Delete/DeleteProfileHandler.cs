using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Repositories;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Delete;

public class DeleteProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ProducerService _producerService) : IRequestHandler<DeleteProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.ProfileId, cancellationToken);
        
        if (profile is null)
        {
            throw new NotFoundException("Profile", request.ProfileId);
        }
        
        await _unitOfWork.ProfileRepository.DeleteProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var message = _mapper.Map<ProfileDeletedMessage>(profile);
        await _producerService.ProduceAsync(message);
        
        return _mapper.Map<ProfileResponseDto>(profile);
    }
}