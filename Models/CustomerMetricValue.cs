using System;
using Newtonsoft.Json;

namespace PSDIPortal.Models
{
    public class CustomerMetricValue
    {
        [JsonProperty(PropertyName = "customer")]
        public string Customer { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public MetricValue[] Metrics { get; set; }
    }
}
