using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class ShoveCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class ShoveCommandValidator : FluentValidatorBase<ShoveCommand>
{
    public ShoveCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
