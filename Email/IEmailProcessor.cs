
namespace PSDIPortal.Email
{
    using PSDIPortal.Models;
    using System;
    using System.Threading.Tasks;

    public interface IEmailProcessor
    {
        /// <summary>Sends the Metrics Anamoly Email reminders.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task SendMetricAnamolyEmail(String user, String metricName, String metricValue);
    }

}