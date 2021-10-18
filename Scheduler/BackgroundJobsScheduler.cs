namespace PSDIPortal.Scheduler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PSDIPortal.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using FluentScheduler;


    public class BackgroundJobsScheduler : IBackgroundJobsScheduler
    {
        private ILogger<BackgroundJobsScheduler> logger;

        /// <inheritdoc/>
        public void start()
        {
            this.logger.LogInformation($"Started Background Scheduler. Scheduled time {DateTime.Now}");

            Action sendMetricsAnamolyEmailReminder = new Action(() =>
            {
                try
                {
                    this.logger.LogDebug($"Invoked sendMetricsAnamolyEmailReminder");
                    //this.emailReminderProcessor.SendAccessExpiryEmailReminder();
                }
                catch (Exception ex)
                {
                    this.logger.LogError("Error occured in sendMetricsAnamolyEmailReminder.", ex);
                }

            });
            var registry = new Registry();
            registry.Schedule(sendMetricsAnamolyEmailReminder).ToRunOnceAt(DateTime.Now).AndEvery(24).Hours();

        }
    }
}
