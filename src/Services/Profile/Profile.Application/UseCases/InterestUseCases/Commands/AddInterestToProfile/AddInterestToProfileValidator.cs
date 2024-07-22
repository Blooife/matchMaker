using FluentValidation;

namespace Profile.Application.UseCases.InterestUseCases.Commands.AddInterestToProfile;

public class AddInterestToProfileValidator : AbstractValidator<AddInterestToProfileCommand>
{
    public AddInterestToProfileValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.InterestId).NotEmpty().WithMessage("Interest id cant be empty");
    }
}