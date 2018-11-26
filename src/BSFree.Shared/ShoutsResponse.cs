using System;
using System.Collections.Generic;

namespace BSFree.Shared
{
    public class ShoutsResponse
    {
        public ShoutsResponse()
        {
            Shouts = new HashSet<Shout>();
        }

        public ICollection<Shout> Shouts { get; private set; }
        public ContinuationToken ContinuationToken { get; set; }
    }

    public class ContinuationToken
    {
        public string NextPartitionKey { get; set; }
        public string NextRowKey { get; set; }
        public int TargetLocation { get; set; }
    }
}