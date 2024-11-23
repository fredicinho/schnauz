using Schnauz.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using FluentValidation;
using Schnauz.Shared;

namespace Schnauz.Client.Components.Validations
{
    public class FluentValidation : ComponentBase
    {
        [Inject] private ValidationProvider _validationProvider { get; set; } = null!;
        [CascadingParameter] private EditContext? EditContext { get; set; }

        private ValidationMessageStore? _validationMessageStore;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            EditContext? previousEditContext = null;
            await base.SetParametersAsync(parameters);
            if (EditContext == null)
            {
                throw new NullReferenceException($"{nameof(FluentValidation)} must be placed within an {nameof(EditForm)}");
            }
            if (EditContext != previousEditContext)
            {
                EditContextChanged();
            }
            previousEditContext = EditContext;
        }

        private void EditContextChanged()
        {
            _validationMessageStore = new ValidationMessageStore(EditContext!);
            EditContext!.OnValidationRequested += ValidationRequested;
            EditContext.OnFieldChanged += FieldChanged;
        }

        private async void ValidationRequested(object? sender, ValidationRequestedEventArgs args)
        {
            _validationMessageStore!.Clear();
            var errors = await _validationProvider.Validate(EditContext!.Model);
            AddValidationResult(errors);
        }

        private async void FieldChanged(object? sender, FieldChangedEventArgs args)
        {
            _validationMessageStore!.Clear(args.FieldIdentifier);
            var errors = (await _validationProvider.Validate(EditContext!.Model))
                            .Where(x => x.ObjectReference == args.FieldIdentifier.Model && x.PropertyName == args.FieldIdentifier.FieldName)
                            .ToList();
            AddValidationResult(errors);
        }

        private void AddValidationResult(IReadOnlyList<ValidationErrorDto> validationErrors)
        {
            foreach (var error in validationErrors)
            {
                var fieldIdentifier = new FieldIdentifier(error.ObjectReference, error.PropertyName);
                if (!EditContext!.GetValidationMessages(fieldIdentifier).Any(x => x == error.ErrorMessage))
                {
                    _validationMessageStore!.Add(fieldIdentifier, error.ErrorMessage);
                }
            }
            EditContext!.NotifyValidationStateChanged();
        }

        public void Dispose()
        {
            if (EditContext != null)
            {
                EditContext.OnValidationRequested -= ValidationRequested;
                EditContext.OnFieldChanged -= FieldChanged;
            }
        }
    }
}
