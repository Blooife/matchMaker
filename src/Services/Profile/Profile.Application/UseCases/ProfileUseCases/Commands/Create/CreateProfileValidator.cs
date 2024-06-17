using FluentValidation;

namespace Profile.Application.UseCases.ProfileUseCases.Commands.Create;

public class CreateProfileValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileValidator()
    {
        RuleFor(command => command.CreateProfileDto.Name).MinimumLength(2).MaximumLength(50).WithMessage("");
        RuleFor(command => command.CreateProfileDto.LastName).MinimumLength(2).MaximumLength(50).WithMessage("");
        RuleFor(command => command.CreateProfileDto.Bio).MinimumLength(50).MaximumLength(500).WithMessage("");
        RuleFor(command => command.CreateProfileDto.Height).GreaterThanOrEqualTo(100).LessThanOrEqualTo(220).WithMessage("");
        RuleFor(command => command.CreateProfileDto.BirthDate)
            .GreaterThanOrEqualTo(DateTime.Today.AddYears(-16))
            .WithMessage("Age must be at least 16 years.");

    }
}