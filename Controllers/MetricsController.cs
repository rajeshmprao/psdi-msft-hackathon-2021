namespace PSDIPortal.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using PSDIPortal.Models;
    using Microsoft.AspNetCore.Http;
    using System.Linq;
    using PSDIPortal.Common;
    [ApiController]
    [Route("api/metrics")]
    public class MetricsController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly IUserProcessor _userProcessor;
        public MetricsController(ICosmosDbService cosmosDbService, IUserProcessor userProcessor)
        {
            _cosmosDbService = cosmosDbService;
            _userProcessor = userProcessor;
        }

        [Route("portfolioMetrics")]
        public async Task<IEnumerable<MetricValue>> GetPortfolioMetrics()
        {
            try
            {
                string currentUser = HttpContext.Session.GetString("CurrentUser");
                User currentUserDetails = (await this._cosmosDbService.GetAsyncByQuery<Models.User>($"select * from c where c.upn='{currentUser}'", Constants.USERS_CONTAINER)).First();
                string query = $"SELECT m['name'], m['value'] FROM c JOIN m in c.metricValues where c.csam='{currentUser}' and c.viewParam='Portfolio'";
                IEnumerable<MetricValue> userMetricValues = (await _cosmosDbService.GetAsyncByQuery<MetricValue>(query, Constants.METRICS_CONTAINER));
                List<MetricValue> result = new List<MetricValue>();
                // Can use a dictionary for metrics customization to speed up.
                foreach (MetricValue mv in userMetricValues)
                {
                    foreach (MetricValue mc in currentUserDetails.MetricsCustomization)
                    {
                        if (mv.Name == mc.Name)
                        {
                            MetricValue temp = new MetricValue();
                            temp.Name = mv.Name;
                            temp.Value = mv.Value;
                            temp.UpperThreshold = mc.UpperThreshold;
                            temp.LowerThreshold = mc.LowerThreshold;
                            result.Add(temp);
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
        [Route("customerMetrics")]
        public async Task<IEnumerable<CustomerMetricValue>> GetCustomerMetrics()
        {
            try
            {
                // List<string> queryMetrics = new List<string>();
                // Console.WriteLine(metricNames);
                string user = HttpContext.Session.GetString("CurrentUser");
                string query = $"SELECT c.customer,Array(SELECT m['name'], m['value'] from c JOIN m in c.metricValues) as metrics FROM c where c.csam='{user}' and c.viewParam='Customer'";
                Console.WriteLine(query);
                IEnumerable<CustomerMetricValue> queryResult = (await _cosmosDbService.GetAsyncByQuery<CustomerMetricValue>(query, Constants.METRICS_CONTAINER));
                return queryResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [Route("allMetrics")]
        public async Task<IEnumerable<string>> GetAllMetrics()
        {
            try
            {
                string query = $"SELECT distinct value m.name FROM c join m in c.metricValues";
                IEnumerable<string> queryResult = (await _cosmosDbService.GetAsyncByQuery<string>(query, Constants.METRICS_CONTAINER));
                return queryResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }

        [Route("addMetric")]
        [HttpPost]
        public async Task AddMetric([FromBody] MetricValue metricDetails)
        {
            try
            {
                // Pull the user document
                User existingUserDetails = await this._userProcessor.getUserDetails();
                List<MetricValue> existingCustomization = existingUserDetails.MetricsCustomization.ToList();
                MetricValue newMetric = new MetricValue();
                newMetric.Name = metricDetails.Name;
                newMetric.UpperThreshold = metricDetails.UpperThreshold;
                newMetric.LowerThreshold = metricDetails.LowerThreshold;
                existingCustomization.Add(newMetric);
                existingUserDetails.MetricsCustomization = existingCustomization.ToArray();
                // Add this metrics

                // Replace document
                await _cosmosDbService.UpdateAsync<User>(existingUserDetails.Id, existingUserDetails, Constants.USERS_CONTAINER);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Route("removeMetric")]
        [HttpGet]
        public async Task RemoveMetric([FromQuery] string metric)
        {
            try
            {
                // Pull the user document
                User existingUserDetails = await this._userProcessor.getUserDetails();
                List<MetricValue> existingCustomization = existingUserDetails.MetricsCustomization.ToList();
                existingUserDetails.MetricsCustomization = existingCustomization.Where(m => m.Name != metric).ToArray();
                // Add this metrics
                // Replace document
                await _cosmosDbService.UpdateAsync<User>(existingUserDetails.Id, existingUserDetails, Constants.USERS_CONTAINER);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}