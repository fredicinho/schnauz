using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class SearchGameCommand : ICommand
{
    public string Username { get; set; } = string.Empty;
}

public class SearchGameCommandValidator : FluentValidatorBase<SignInCommand>
{
    public SearchGameCommandValidator()
    {
        RuleFor(x => x.Username).MinimumLength(3).MaximumLength(40);
    }
}
