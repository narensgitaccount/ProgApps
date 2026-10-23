namespace ADE.ESS.SEPA.Client.ErrorHandling
{
    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public interface INotificationService
    {
        event Action<string, NotificationType>? OnNotify;
        void ShowSuccess(string message);
        void ShowError(string message);
        void ShowWarning(string message);
        void ShowInfo(string message);
    }
}
