using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BSFree.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BSFree.Helpers
{
    public interface IPaginationService
    {
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        Task<Shout[]> GetPreviousShoutsPage();
        Task<Shout[]> GetNextShoutsPage();
    }

    public class PaginationService : IPaginationService
    {
        private readonly IApiClient _apiClient;
        private readonly IContinuationTokenHelper _continuationTokenHelper;

        public bool HasPreviousPage =>
            _continuationTokenHelper.ShoutsPages.Count() > 1;
        public bool HasNextPage =>
            _continuationTokenHelper.GetNextPageToken() != null;

        public PaginationService(IApiClient apiClient, IContinuationTokenHelper continuationTokenHelper)
        {
            _apiClient = apiClient;
            _continuationTokenHelper = continuationTokenHelper;
        }

        public async Task<Shout[]> GetNextShoutsPage()
        {
            var token = _continuationTokenHelper.GetNextPageToken();
            var response = await _apiClient.GetShoutsResponse(token);

            _continuationTokenHelper.AddToken(response.ContinuationToken);
            return response.Shouts.ToArray();
        }

        public async Task<Shout[]> GetPreviousShoutsPage()
        {
            var token = _continuationTokenHelper.GetPreviousPageToken();
            var response = await _apiClient.GetShoutsResponse(token);

            return response.Shouts.ToArray();
        }
    }
}
