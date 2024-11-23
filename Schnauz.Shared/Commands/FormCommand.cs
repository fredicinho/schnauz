using Schnauz.Shared.Interfaces;
using FluentValidation;

namespace Schnauz.Shared.Commands;
public class FormCommand : ICommand
{
    public int Number { get; set; }
    public bool Checkbox { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime? DateTime { get; set; }
}

public class FormCommandValidator : FluentValidatorBase<FormCommand>
{
    public FormCommandValidator()
    {
        RuleFor(x => x.Text).MinimumLength(3).MaximumLength(10);
        RuleFor(x => x.Number).GreaterThanOrEqualTo(10).LessThan(100);
        RuleFor(x => x.Checkbox).NotEmpty();
        RuleFor(x => x.DateTime).NotEmpty().GreaterThanOrEqualTo(DateTime.Now);
    }
}
