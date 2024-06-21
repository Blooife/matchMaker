using FluentValidation;

namespace Profile.Application.UseCases.EducationUseCases.Commands.Update;

public class UpdateProfileEducationValidator : AbstractValidator<UpdateProfileEducationCommand>
{
    public UpdateProfileEducationValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        RuleFor(command => command.Dto.EducationId).NotEmpty().WithMessage("Education id cant be empty");
        RuleFor(command => command.Dto.Description).MinimumLength(2).MaximumLength(100).WithMessage("Description mut be between 2 and 100 characters");
    }
}