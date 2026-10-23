namespace ADE.ESS.SEPA.Client.ErrorHandling
{
    public class NotificationService : INotificationService
    {
        public event Action<string, NotificationType>? OnNotify;

        public void ShowSuccess(string message)
        {
            OnNotify?.Invoke(message, NotificationType.Success);
        }

        public void ShowError(string message)
        {
            OnNotify?.Invoke(message, NotificationType.Error);
        }

        public void ShowWarning(string message)
        {
            OnNotify?.Invoke(message, NotificationType.Warning);
        }

        public void ShowInfo(string message)
        {
            OnNotify?.Invoke(message, NotificationType.Info);
        }
    }
}
