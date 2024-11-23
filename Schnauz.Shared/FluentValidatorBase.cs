using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Schnauz.Shared
{
    public abstract class FluentValidatorBase<T> : AbstractValidator<T>
    {
        public FluentValidatorBase()
        {
            //ValidatorOptions.Global.LanguageManager.Culture = CultureInfo.GetCultureInfo("cs");
            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                var attribute = (DisplayAttribute?)memberInfo?.GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault() ?? null;
                return attribute?.Name ?? memberInfo!.Name;
            };
        }
    }
}
