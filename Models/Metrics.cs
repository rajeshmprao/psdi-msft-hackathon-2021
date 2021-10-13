using System;
using Newtonsoft.Json;

namespace PSDIPortal.Models
{
    public class Metrics
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "csam")]
        public string CSAM { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public string Customer { get; set; }

        [JsonProperty(PropertyName = "viewParam")]
        public string ViewParam { get; set; } // Portfolio if at portfolio grain, Customer if at customer grain

        [JsonProperty(PropertyName = "metricValues")]
        public MetricValue[] MetricValues { get; set; }
    }
}
