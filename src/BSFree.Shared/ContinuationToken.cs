namespace BSFree.Shared
{
    public class ContinuationToken
    {
        public string NextPartitionKey { get; set; }
        public string NextRowKey { get; set; }
        public int TargetLocation { get; set; }
    }
}