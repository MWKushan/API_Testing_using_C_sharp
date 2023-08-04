using Newtonsoft.Json;

namespace UnitTests
{
    public class Data
    {
        public string color { get; set; }
        public string capacity { get; set; }
        public string price { get; set; }

        [JsonProperty("Screen size")]
        public string screenSize { get; set; }
        [JsonProperty("capacity GB")]
        public string capacityGb { get; set; }
        public string year { get; set; }
        [JsonProperty("CPU model")]
        public string cpuModel { get; set; }
        [JsonProperty("Hard disk size")]
        public string hardDiskSize { get; set; }
    }
}
