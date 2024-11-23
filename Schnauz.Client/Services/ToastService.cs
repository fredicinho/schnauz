using Schnauz.Client.Components.Toastr;
using Schnauz.Shared;

namespace Schnauz.Client.Services
{
    public class ToastService : IInfoMessage
    {
        public ToastService() { }

        /// <summary>
        /// A event that will be invoked when showing a toast
        /// </summary>
        public event Action<ToastLevel, string, string>? OnShow;

        public void ShowInfo(string message, string heading = "")
        {
            ShowToast(ToastLevel.Info, message, heading);
        }

        public void ShowSuccess(string message, string heading = "")
        {
            ShowToast(ToastLevel.Success, message, heading);
        }

        public void ShowWarning(string message, string heading = "")
        {
            ShowToast(ToastLevel.Warning, message, heading);
        }

        public void ShowError(string message, string heading = "")
        {
            ShowToast(ToastLevel.Error, message, heading);
        }

        public void ShowToast(ToastLevel level, string message, string heading = "")
        {
            OnShow?.Invoke(level, message, heading);
            Console.WriteLine($"{level} Message: {message}");
        }
    }
}
