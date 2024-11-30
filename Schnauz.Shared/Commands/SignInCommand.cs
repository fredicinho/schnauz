using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class SignInCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class SignInCommandValidator : FluentValidatorBase<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
