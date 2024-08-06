using FluentValidation;

namespace Profile.Application.UseCases.LanguageUseCases.Commands.AddLanguageToProfile;

public class AddLanguageToProfileValidator : AbstractValidator<AddLanguageToProfileCommand>
{
    public AddLanguageToProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.LanguageId).NotEmpty().WithMessage("Language id cant be empty");
    }
}