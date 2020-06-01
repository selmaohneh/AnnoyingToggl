using System;
using System.Collections.Generic;
using AnnoyingToggl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Toggl;
using Toggl.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace AnnoyingTogglTests
{
    [TestClass]
    public class AnnoyServiceTests
    {
        [TestMethod]
        public async Task StartAnnoyService_RunningTimer_BalloonDescription()
        {
            var timeEntries = new List<TimeEntry>
            {
                new TimeEntry
                {
                    Duration = 99,
                    Description = "Watching Little Britain."
                },

                new TimeEntry
                {
                    Duration = -42,
                    Description = "Writing unit tests."
                },

                new TimeEntry
                {
                    Duration = 42,
                    Description = "Drinking coffee."
                }
            };
            var timeEntryService = new Mock<ITimeEntryService>();
            timeEntryService.Setup(x => x.List()).Returns(timeEntries);

            var balloonNotification = new Mock<IBalloonNotification>();

            var sut = new AnnoyService(timeEntryService.Object, balloonNotification.Object);

            sut.Start(TimeSpan.FromSeconds(1));

            await Task.Delay(TimeSpan.FromSeconds(1));

            balloonNotification.Verify(x =>
                x.Show("Writing unit tests.", "That's not what you are doing right now? Toggl your work now!"));
        }

        [TestMethod]
        public async Task StartAnnoyService_NoRunningTimer_BalloonDescription()
        {
            var timeEntries = new List<TimeEntry>
            {
                new TimeEntry
                {
                    Duration = 99,
                    Description = "Watching Little Britain."
                },
                new TimeEntry
                {
                    Duration = 42,
                    Description = "Drinking coffee."
                }
            };
            var timeEntryService = new Mock<ITimeEntryService>();
            timeEntryService.Setup(x => x.List()).Returns(timeEntries);

            var balloonNotification = new Mock<IBalloonNotification>();

            var sut = new AnnoyService(timeEntryService.Object, balloonNotification.Object);

            sut.Start(TimeSpan.FromSeconds(1));

            await Task.Delay(TimeSpan.FromSeconds(1));

            balloonNotification.Verify(x =>
                x.Show("You have no active timer at the moment.", "Toggl your work now!"));
        }

        [TestMethod]
        public async Task StartAnnoyService_Exception_BalloonDescription()
        {
            var timeEntryService = new Mock<ITimeEntryService>();
            timeEntryService.Setup(x => x.List()).Throws<Exception>();

            var balloonNotification = new Mock<IBalloonNotification>();

            var sut = new AnnoyService(timeEntryService.Object, balloonNotification.Object);

            sut.Start(TimeSpan.FromSeconds(1));

            await Task.Delay(TimeSpan.FromSeconds(1));

            balloonNotification.Verify(x =>
                x.Show("Could not retrieve time entries from Toggl.",
                    "Make sure your internet connection is up and your Toggl API key is valid."));
        }
    }
}