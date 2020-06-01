using System.Windows.Forms;

namespace AnnoyingToggl
{
    public class NotifyIconBalloonNotification : IBalloonNotification
    {
        private readonly NotifyIcon _notifyIcon;

        public NotifyIconBalloonNotification(NotifyIcon notifyIcon)
        {
            _notifyIcon = notifyIcon;
        }

        public void Show(string title, string text)
        {
            _notifyIcon.ShowBalloonTip(5000, title, text, ToolTipIcon.None);
        }
    }
}