namespace Schnauz.Client.Components.Toastr
{
    public class ToastModel
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ToastLevel Level { get; set; } = ToastLevel.Info;
    }
}
