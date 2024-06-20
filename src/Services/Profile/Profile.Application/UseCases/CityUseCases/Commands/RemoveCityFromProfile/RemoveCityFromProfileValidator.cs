using FluentValidation;

namespace Profile.Application.UseCases.CityUseCases.Commands.RemoveCityFromProfile;

public class RemoveCityFromProfileValidator : AbstractValidator<RemoveCityFromProfileCommand>
{
    public RemoveCityFromProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
    }
}