namespace Schnauz.Shared;

public interface IInfoMessage
{
    public void ShowError(string message, string heading);
    public void ShowInfo(string message, string heading);
    public void ShowSuccess(string message, string heading);
    public void ShowWarning(string message, string heading);
}
