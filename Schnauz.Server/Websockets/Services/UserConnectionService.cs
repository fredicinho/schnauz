namespace Schnauz.Server.Websockets.Services;

public class UserConnectionService
{
    private readonly Dictionary<string, string> _userConnections = new();

    public void AddConnection(string userName, string connectionId)
    {
        _userConnections[userName] = connectionId;
    }

    public bool TryGetConnection(string userName, out string? connectionId)
    {
        return _userConnections.TryGetValue(userName, out connectionId);
    }

    public void RemoveConnection(string connectionId)
    {
        var item = _userConnections.FirstOrDefault(x => x.Value == connectionId);
        if (!string.IsNullOrEmpty(item.Key))
        {
            _userConnections.Remove(item.Key);
        }
    }
}
