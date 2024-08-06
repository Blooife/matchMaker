using FluentValidation;

namespace Match.Application.UseCases.ChatUseCases.Commands.Delete;

public class DeleteChatValidator : AbstractValidator<DeleteChatCommand>
{
    public DeleteChatValidator()
    {
        RuleFor(command => command.ChatId).NotEmpty();
    }
}