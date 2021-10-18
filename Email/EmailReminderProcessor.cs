
namespace PSDIPortal.Email
{

    using PSDIPortal.Common;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PSDIPortal.Models;
    using System;

    public class EmailReminderProcessor : IEmailReminderProcessor
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly IUserProcessor _userProcessor;
        private readonly IEmailProcessor emailProcessor;
        private object logger;

        public EmailReminderProcessor(ICosmosDbService cosmosDbService, IUserProcessor userProcessor)
        {
            _cosmosDbService = cosmosDbService;
            _userProcessor = userProcessor;
        }
        public async Task SendMetricAnamolyEmailReminder()
        {
            string metricAnamolyQuery = $"SELECT * FROM c ";
            IEnumerable<User> usersMetricValues = (await _cosmosDbService.GetAsyncByQuery<User>(metricAnamolyQuery, Constants.METRICS_CONTAINER));

            foreach(var request in usersMetricValues)
            {
                var user = request.UPN;
                foreach( var metric in request.MetricsCustomization)
                {
                    if(metric.Subscribe == "1")
                    {
                        if((int.Parse(metric.Value) <  int.Parse(metric.LowerThreshold)) || (int.Parse(metric.Value) > int.Parse(metric.UpperThreshold) ))
                        {
                            await this.emailProcessor.SendMetricAnamolyEmail(user,metric.Name,metric.Value);

                        }
                    }
                }
            }

        }
    }

}