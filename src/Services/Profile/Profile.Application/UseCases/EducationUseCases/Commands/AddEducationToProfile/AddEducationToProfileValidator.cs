using FluentValidation;

namespace Profile.Application.UseCases.EducationUseCases.Commands.AddEducationToProfile;

public class AddEducationToProfileValidator : AbstractValidator<AddEducationToProfileCommand>
{
    public AddEducationToProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.EducationId).NotEmpty().WithMessage("Education id cant be empty");
        
        RuleFor(command => command.Dto.Description).MinimumLength(0).MaximumLength(100).WithMessage("Description mut be between 0 and 100 characters");
    }
}