using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Models;
using Profile.Domain.Interfaces;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ProducerService _producerService) : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = _mapper.Map<UserProfile>(request.CreateProfileDto);
        var result = await _unitOfWork.ProfileRepository.CreateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var message = _mapper.Map<ProfileCreatedMessage>(profile);
        var city = await _unitOfWork.CityRepository.GetCityWithCountryById(profile.CityId, cancellationToken);
        message.City = city.Name;
        message.Country = city.Country.Name;
        await _producerService.ProduceAsync(message);
        
            
        return _mapper.Map<ProfileResponseDto>(result);
    }
}