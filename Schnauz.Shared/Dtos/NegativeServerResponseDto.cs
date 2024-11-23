namespace Schnauz.Shared.Dtos;
public class NegativeServerResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public IReadOnlyList<ValidationErrorDto>? ValidationErrors { get; set; }
}
