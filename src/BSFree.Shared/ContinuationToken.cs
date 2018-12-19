using Newtonsoft.Json;

namespace BSFree.Shared
{
    public class ContinuationToken
    {
        [JsonProperty("nextPartitionKey")]
        public string NextPartitionKey { get; set; }

        [JsonProperty("nextRowKey")]
        public string NextRowKey { get; set; }

        [JsonProperty("targetLocation")]
        public int TargetLocation { get; set; }
    }
}