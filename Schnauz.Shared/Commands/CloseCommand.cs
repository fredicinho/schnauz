using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class CloseCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class CloseCommandValidator : FluentValidatorBase<CloseCommand>
{
    public CloseCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
