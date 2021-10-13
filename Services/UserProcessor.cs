namespace PSDIPortal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PSDIPortal.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.AspNetCore.Http;
    using System;
    using Common;
    public class UserProcessor : IUserProcessor
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ICosmosDbService _cosmosDbService;

        public UserProcessor(IHttpContextAccessor httpContextAccessor, ICosmosDbService cosmosDbService)
        {
            _httpContextAccessor = httpContextAccessor;
            _cosmosDbService = cosmosDbService;
        }
        public async Task AddDefaultCurrentUserDetails()
        {
            // Create a new model for user and populate it
            Models.User currentUserDefaultModel = new Models.User();
            List<MetricValue> currentUserDefaultMetrics = new List<MetricValue>();

            MetricValue defaultMetric1 = new MetricValue();
            defaultMetric1.Name = "BurnRate";
            defaultMetric1.LowerThreshold = "30";
            defaultMetric1.UpperThreshold = "70";

            MetricValue defaultMetric2 = new MetricValue();
            defaultMetric2.Name = "SoldHours";
            defaultMetric2.LowerThreshold = "100";
            defaultMetric2.UpperThreshold = "200";

            currentUserDefaultMetrics.Add(defaultMetric1);
            currentUserDefaultMetrics.Add(defaultMetric2);

            currentUserDefaultModel.UPN = _httpContextAccessor.HttpContext.Session.GetString("CurrentUser");
            currentUserDefaultModel.Id = Guid.NewGuid().ToString();
            currentUserDefaultModel.MetricsCustomization = currentUserDefaultMetrics.ToArray();

            await this._cosmosDbService.AddAsync<Models.User>(currentUserDefaultModel, Constants.USERS_CONTAINER);
        }
        public async Task<bool> checkIsCurrentUserNew()
        {
            string currentUser = _httpContextAccessor.HttpContext.Session.GetString("CurrentUser");
            var currentUserDetails = await getUserDetails();
            bool result = currentUserDetails == null;
            return result;

        }

        public async Task<Models.User> getUserDetails()
        {
            string currentUser = _httpContextAccessor.HttpContext.Session.GetString("CurrentUser");
            var userResult = (await this._cosmosDbService.GetAsyncByQuery<Models.User>($"select * from c where c.upn='{currentUser}'", Constants.USERS_CONTAINER));
            if (userResult.Count() > 0)
            {
                return userResult.First();
            }
            else return null;


        }


    }
}