using AutoMapper;
using MediatR;
using Profile.Application.DTOs.Profile.Response;
using Profile.Application.Exceptions;
using Profile.Application.Kafka.Producers;
using Profile.Domain.Models;
using Profile.Domain.Repositories;
using Shared.Messages.Profile;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileHandler(IUnitOfWork _unitOfWork, IMapper _mapper, ProducerService _producerService) : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
{
    public async Task<ProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var findRes =
            await _unitOfWork.ProfileRepository.FirstOrDefaultAsync(request.UpdateProfileDto.Id, cancellationToken);
        
        if (findRes is null)
        {
            throw new NotFoundException("Profile", request.UpdateProfileDto.Id);
        }
        
        var profile = _mapper.Map<UserProfile>(request.UpdateProfileDto);
        var result = await _unitOfWork.ProfileRepository.UpdateProfileAsync(profile, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        
        var message = _mapper.Map<ProfileUpdatedMessage>(result);
        var city = await _unitOfWork.CityRepository.GetCityWithCountryById(result.CityId, cancellationToken);
        message.City = city.Name;
        message.Country = city.Country.Name;
        await _producerService.ProduceAsync(message);
        
        return _mapper.Map<ProfileResponseDto>(result);
    }
}