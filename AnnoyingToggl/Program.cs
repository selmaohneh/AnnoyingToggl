using System;
using System.Windows.Forms;
using AnnoyingToggl.Properties;
using Toggl.Services;

namespace AnnoyingToggl
{
    public static class Program
    {
        private static NotifyIcon _notifyIcon;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(params string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please provide your toggl API key as application argument.");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _notifyIcon = new NotifyIcon
            {
                Icon = Resources.time,
                Visible = true,
                ContextMenu = new ContextMenu
                {
                    MenuItems =
                    {
                        new MenuItem("Exit", OnExitClick)
                    }
                }
            };

            var timeEntryService = new TimeEntryService(args[0]);

            var notifyIconBalloonNotification = new NotifyIconBalloonNotification(_notifyIcon);
            var annoyService = new AnnoyService(timeEntryService, notifyIconBalloonNotification);

            annoyService.Start(TimeSpan.FromMinutes(5));

            Application.Run();
        }

        private static void OnExitClick(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            Application.Exit();
        }
    }
}