using System;
using System.Collections.Generic;
using System.Linq;
using Toggl;
using Toggl.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace AnnoyingToggl
{
    public class AnnoyService
    {
        private readonly ITimeEntryService _timeEntryService;
        private readonly IBalloonNotification _balloonNotification;

        public AnnoyService(ITimeEntryService timeEntryService, IBalloonNotification balloonNotification)
        {
            _timeEntryService = timeEntryService;
            _balloonNotification = balloonNotification;
        }

        public async Task Start(TimeSpan interval)
        {
            while (true)
            {
                List<TimeEntry> timeEntries;

                try
                {
                    timeEntries = _timeEntryService.List();
                }
                catch (Exception)
                {
                    _balloonNotification.Show("Could not retrieve time entries from Toggl.",
                        "Make sure your internet connection is up and your Toggl API key is valid.");
                    return;
                }

                var currentEntry = timeEntries.SingleOrDefault(x => x.Duration < 0);

                if (currentEntry == null)
                {
                    _balloonNotification.Show("You have no active timer at the moment.", "Toggl your work now!");
                }
                else
                {
                    _balloonNotification.Show(currentEntry.Description,
                        "That's not what you are doing right now? Toggl your work now!");
                }

                await Task.Delay(interval);
            }
        }
    }
}