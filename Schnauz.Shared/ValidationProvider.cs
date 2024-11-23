using FluentValidation;
using System.Collections;
using Schnauz.Shared.Dtos;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Schnauz.Shared
{
    public class ValidationProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public ValidationProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<IReadOnlyList<ValidationErrorDto>> Validate(object obj)
        {
            List<ValidationErrorDto> errors;
            if (obj.GetType().IsCollection())
            {
                errors = await ValidateCollection((ICollection)obj, "", "");
            }
            else
            {
                errors = await ValidateObject(obj, "");
                errors.AddRange(await ValidateObjectRecursive(obj, ""));
            }
            return errors;
        }

        private async Task<List<ValidationErrorDto>> ValidateObject(object obj, string prefix)
        {
            prefix = string.IsNullOrWhiteSpace(prefix) ? obj.GetType().Name : $"{prefix}.{obj.GetType().Name}";
            var errors = new List<ValidationErrorDto>();
            if (obj == null)
            {
                return errors;
            }

            var genericValidatorType = typeof(IValidator<>).MakeGenericType(obj.GetType());
            var validator = (IValidator)_serviceProvider.GetService(genericValidatorType)!;
            if (validator == null)
            {
                return errors;
            }

            var result = await validator.ValidateAsync(new ValidationContext<object>(obj));
            errors.AddRange(result.Errors.Select(o => new ValidationErrorDto
            {
                ErrorMessage = o.ErrorMessage,
                PropertyFullName = $"{prefix}.{o.PropertyName}",
                ObjectReference = GetObjectReference(obj, o.PropertyName)
            }));
            return errors;
        }

        private async Task<List<ValidationErrorDto>> ValidateObjectRecursive(object obj, string prefix)
        {
            if (obj == null)
            {
                return new List<ValidationErrorDto>();
            }

            var objType = obj.GetType();
            var currentPrefix = string.IsNullOrWhiteSpace(prefix) ? objType.Name : $"{prefix}.{objType.Name}";
            var errors = new List<ValidationErrorDto>();
            foreach (var propertyInfo in objType.GetProperties().Where(x => x.IsClassOrCollectionOfClasses()))
            {
                var toValidate = propertyInfo.GetValue(obj);
                if (toValidate != null)
                {
                    if (propertyInfo.PropertyType.IsCollection())
                    {
                        errors.AddRange(await ValidateCollection((ICollection)toValidate, currentPrefix, propertyInfo.Name));
                    }
                    else
                    {
                        errors.AddRange(await ValidateObject(toValidate, currentPrefix));
                        errors.AddRange(await ValidateObjectRecursive(toValidate, currentPrefix));
                    }
                }
            }

            return errors;
        }

        private async Task<List<ValidationErrorDto>> ValidateCollection(ICollection collection, string currentPrefix, string objectName)
        {
            var errors = new List<ValidationErrorDto>();
            var newPrefix = string.IsNullOrWhiteSpace(currentPrefix) ? objectName : currentPrefix + "." + objectName;
            foreach (var item in collection)
            {
                if (item != null)
                {
                    errors.AddRange(await ValidateObject(item, newPrefix));
                    errors.AddRange(await ValidateObjectRecursive(item, newPrefix));
                }
            }
            return errors;
        }

        private static object GetObjectReference(object obj, string propertyName)
        {
            var objectNames = propertyName.Split('.');
            objectNames = objectNames.Take(objectNames.Length - 1).ToArray();
            object currentObject = obj;
            foreach (var name in objectNames)
            {
                if (name.EndsWith("]"))
                {
                    var split = name.Split('[');
                    var collection = currentObject!.GetType().GetProperty(split[0])!.GetValue(currentObject);
                    var searchIndex = int.Parse(split[1].Replace("]", ""));
                    var currentIndex = 0;
                    foreach (var item in (ICollection)collection!)
                    {
                        if (searchIndex == currentIndex)
                        {
                            return item;
                        }
                        currentIndex++;
                    }
                }
                else
                {
                    var property = currentObject!.GetType().GetProperty(name);
                    currentObject = property!.GetValue(currentObject)!;
                }
            }
            return currentObject;
        }
    }

    public static class ValidationProviderHelper
    {
        public static bool IsCollection(this Type type)
        {
            return typeof(ICollection).IsAssignableFrom(type);
        }

        public static bool IsClassOrCollectionOfClasses(this PropertyInfo property)
        {
            return property.CanRead
                && property.PropertyType != typeof(string)
                && (property.PropertyType.IsClass
                    && !property.PropertyType.IsCollection()
                    || (property.PropertyType.IsCollection()
                            && property.PropertyType.GetGenericArguments().Any()
                            && property.PropertyType.GetGenericArguments()[0].IsClass
                            && property.PropertyType.GetGenericArguments()[0] != typeof(string)
                        )
                    );
        }

        public static IServiceCollection AddFluentValidations(this IServiceCollection services, Type assemblyPivot)
        {
            return services.AddFluentValidations([assemblyPivot]);
        }

        public static IServiceCollection AddFluentValidations(this IServiceCollection services, Type[] assemblyPivots)
        {
            services.AddScoped(x => new ValidationProvider(x));

            foreach (var assemblyPivot in assemblyPivots)
            {
                foreach (var (iFace, implementation) in InterfaceMappingHelper.MappingByConventionBasedOnInterface(typeof(IValidator<>), assemblyPivot))
                {
                    services.AddTransient(iFace, implementation);
                }
            }

            return services;
        }
    }
}
