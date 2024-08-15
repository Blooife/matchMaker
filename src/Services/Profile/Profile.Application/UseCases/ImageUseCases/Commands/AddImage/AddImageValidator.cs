using FluentValidation;

namespace Profile.Application.UseCases.ImageUseCases.Commands.AddImage;

public class AddImageValidator : AbstractValidator<AddImageCommand>
{
    public AddImageValidator()
    {
        RuleFor(command => command.Dto.ProfileId).NotEmpty().WithMessage("Profile id cant be empty");
        
        RuleFor(command => command.Dto.file).NotEmpty().WithMessage("You must upload file");
    }
}