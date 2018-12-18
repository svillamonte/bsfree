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

        public ICollection<Shout> Shouts { get; set; }
        public ContinuationToken ContinuationToken { get; set; }
    }
}