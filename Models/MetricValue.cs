using System;
using Newtonsoft.Json;

namespace PSDIPortal.Models
{
    public class MetricValue
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "upperThreshold")]
        public string UpperThreshold { get; set; }

        [JsonProperty(PropertyName = "lowerThreshold")]
        public string LowerThreshold { get; set; }
    }
}
