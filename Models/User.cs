using System;
using Newtonsoft.Json;

namespace PSDIPortal.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "upn")]
        public string UPN { get; set; }


        [JsonProperty(PropertyName = "metricsCustomization")]
        public MetricValue[] MetricsCustomization { get; set; }
    }
}
