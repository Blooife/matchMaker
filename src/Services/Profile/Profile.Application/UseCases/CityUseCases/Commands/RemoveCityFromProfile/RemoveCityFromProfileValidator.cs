using FluentValidation;
using Profile.Application.DTOs.City.Request;

namespace Profile.Application.UseCases.CityUseCases.Commands.RemoveCityFromProfile;

public class RemoveCityFromProfileValidator : AbstractValidator<RemoveCityFromProfileCommand>
{
    public RemoveCityFromProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
    }
}