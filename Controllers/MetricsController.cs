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
                string user = HttpContext.Session.GetString("CurrentUser");
                User currentUserDetails = (await this._cosmosDbService.GetAsyncByQuery<Models.User>($"select * from c where c.upn='{user}'", Constants.USERS_CONTAINER)).First();

                string query = $"SELECT c.customer,Array(SELECT m['name'], m['value'] from c JOIN m in c.metricValues) as metrics FROM c where c.csam='{user}' and c.viewParam='Customer'";
                IEnumerable<CustomerMetricValue> customerMetricValues = (await _cosmosDbService.GetAsyncByQuery<CustomerMetricValue>(query, Constants.METRICS_CONTAINER));
                // The final List of CustomerMetricValue which we will return
                List<CustomerMetricValue> result = new List<CustomerMetricValue>();
                // Iterate through each customer's metrics
                foreach (CustomerMetricValue cmv in customerMetricValues)
                {
                    // The List of all metrics for the current customer.
                    List<MetricValue> currentCustomerMetrics = new List<MetricValue>();
                    // Iterate through each metric of current customer.
                    foreach (MetricValue mv in cmv.Metrics)
                    {
                        // Iterate through each metrics' user customization 
                        foreach (MetricValue umv in currentUserDetails.MetricsCustomization)
                        {
                            // If the customization name matches the current customers current metric, then create a new metricvalue and add it to the list of metrics for current customer
                            if (mv.Name == umv.Name)
                            {
                                MetricValue tempMetricValue = new MetricValue();
                                tempMetricValue.Name = mv.Name;
                                tempMetricValue.Value = mv.Value;
                                tempMetricValue.UpperThreshold = umv.UpperThreshold;
                                tempMetricValue.LowerThreshold = umv.LowerThreshold;
                                currentCustomerMetrics.Add(tempMetricValue);
                            }
                        }
                    }
                    // After iterating through all metrics of current customer, append the list of metrics to the final customer list.
                    CustomerMetricValue tempCustomerMertricValue = new CustomerMetricValue();
                    tempCustomerMertricValue.Customer = cmv.Customer;
                    tempCustomerMertricValue.Metrics = currentCustomerMetrics.ToArray();
                    result.Add(tempCustomerMertricValue);
                }
                return result;
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