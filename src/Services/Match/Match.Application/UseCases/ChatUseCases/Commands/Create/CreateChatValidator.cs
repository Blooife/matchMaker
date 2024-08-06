using FluentValidation;

namespace Match.Application.UseCases.ChatUseCases.Commands.Create;

public class CreateChatValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatValidator()
    {
        RuleFor(command => command.Dto.FirstProfileId).NotEmpty();
        RuleFor(command => command.Dto.SecondProfileId).NotEmpty();
    }
}