using System.Collections.Generic;
using System.Linq;
using BSFree.Shared;

namespace BSFree.Helpers
{
    public interface IPaginationHelper
    {
        Stack<ShoutsPage> ShoutsPages { get; }
        bool HasNextPage { get; }
        bool HasPreviousPage { get; }

        ContinuationToken GetNextPageToken();
        ContinuationToken GetPreviousPageToken();
        void AddToken(ContinuationToken continuationToken);
    }

    public class PaginationHelper : IPaginationHelper
    {
        public Stack<ShoutsPage> ShoutsPages { get; }
        public bool HasNextPage => NextPageToken != null;
        public bool HasPreviousPage => ShoutsPages.Count() > 1;

        private ContinuationToken NextPageToken { get; set; }

        public PaginationHelper()
            => ShoutsPages = new Stack<ShoutsPage>();

        public ContinuationToken GetNextPageToken()
        {
            return NextPageToken;
        }

        public ContinuationToken GetPreviousPageToken()
        {
            if (!ShoutsPages.Any())
            {
                return null;
            }

            var lastPage = ShoutsPages.Pop();
            NextPageToken = lastPage.PaginationToken;

            return ShoutsPages.Peek().PaginationToken;
        }

        public void AddToken(ContinuationToken continuationToken)
        {
            var nextIndex = 0;
            if (ShoutsPages.Any())
            {
                nextIndex = ShoutsPages.Peek().Index + 1;
            }

            ShoutsPages.Push(new ShoutsPage
            {
                Index = nextIndex,
                PaginationToken = NextPageToken
            });

            NextPageToken = continuationToken;
        }
    }
}