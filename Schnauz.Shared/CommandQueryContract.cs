using System.Text.Json;

namespace Schnauz.Shared;

public class CommandQueryContract
{
    public CommandQueryContract()
    {
    }

    public CommandQueryContract(object query)
    {
        Json = JsonSerializer.Serialize(query);
        ObjectName = query.GetType().AssemblyQualifiedName ?? string.Empty;
        SimpleName = query.GetType().Name;
    }

    public string Json { get; set; } = string.Empty;
    public string ObjectName { get; set; } = string.Empty;
    public string SimpleName { get; set; } = string.Empty;

    public object GetObject() => JsonSerializer.Deserialize(Json, Type.GetType(ObjectName)!)!;
}
