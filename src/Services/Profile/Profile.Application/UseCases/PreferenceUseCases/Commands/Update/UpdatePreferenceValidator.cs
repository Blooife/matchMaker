using FluentValidation;

namespace Profile.Application.UseCases.PreferenceUseCases.Commands.Update;

public class UpdatePreferenceValidator : AbstractValidator<UpdatePreferenceCommand>
{
    public UpdatePreferenceValidator()
    {
        RuleFor(command => command.Dto.MaxDistance).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("Max distance must be >= 0");
        RuleFor(command => command.Dto.AgeFrom).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("Age from must be >= 0");
        RuleFor(command => command.Dto.AgeTo).NotEmpty().GreaterThanOrEqualTo(0).WithMessage("Age to must be >= 0");
    }
}