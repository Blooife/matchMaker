using FluentValidation;
using Profile.Application.UseCases.PreferenceUseCases.Commands.Update;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Update;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileValidator()
    {
        RuleFor(command => command.UpdateProfileDto.Name).MinimumLength(2).MaximumLength(50).WithMessage("");
        RuleFor(command => command.UpdateProfileDto.LastName).MinimumLength(2).MaximumLength(50).WithMessage("");
        RuleFor(command => command.UpdateProfileDto.Bio).MinimumLength(50).MaximumLength(500).WithMessage("");
        RuleFor(command => command.UpdateProfileDto.Height).GreaterThanOrEqualTo(100).LessThanOrEqualTo(220).WithMessage("");
        RuleFor(command => command.UpdateProfileDto.BirthDate)
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Age must be at least 16 years.");
    }
}