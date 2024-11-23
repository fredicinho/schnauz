using System.Text.Json.Serialization;

namespace Schnauz.Shared.Dtos;

public class ValidationErrorDto
{
    public static string DefaultValidationErrorMessage = "Validation failed";
    public string PropertyFullName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    [JsonIgnore]
    public object ObjectReference { get; set; } = null!;

    [JsonIgnore]
    public string PropertyName => PropertyFullName.Split('.').Last();
}
