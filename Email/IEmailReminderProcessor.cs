
namespace PSDIPortal.Email
{
    using System.Threading.Tasks;

    public interface IEmailReminderProcessor
    {
        /// <summary>Sends the Metrics Anamoly Email reminders.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        Task SendMetricAnamolyEmailReminder();
    }

}