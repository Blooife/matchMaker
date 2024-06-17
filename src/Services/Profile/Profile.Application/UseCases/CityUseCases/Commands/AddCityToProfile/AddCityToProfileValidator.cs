using FluentValidation;

namespace Profile.Application.UseCases.CityUseCases.Commands.AddCityToProfile;

public class AddCityToProfileValidator : AbstractValidator<AddCityToProfileCommand>
{
    public AddCityToProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        RuleFor(command => command.Dto.CityId).NotEmpty().WithMessage("City id cant be empty");
    }
}