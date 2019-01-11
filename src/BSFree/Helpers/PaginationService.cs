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
        private readonly HttpClient _httpClient;
        private readonly IContinuationTokenHelper _continuationTokenHelper;

        public bool HasPreviousPage =>
            _continuationTokenHelper.ShoutsPages.Count() > 1;
        public bool HasNextPage =>
            _continuationTokenHelper.GetNextPageToken() != null;

        public PaginationService(HttpClient httpClient, IContinuationTokenHelper continuationTokenHelper)
        {
            _httpClient = httpClient;
            _continuationTokenHelper = continuationTokenHelper;
        }

        public async Task<Shout[]> GetNextShoutsPage()
        {
            var token = _continuationTokenHelper.GetNextPageToken();
            var response = await GetShoutsResponse(token);

            _continuationTokenHelper.AddToken(response.ContinuationToken);
            return response.Shouts.ToArray();
        }

        public async Task<Shout[]> GetPreviousShoutsPage()
        {
            var token = _continuationTokenHelper.GetPreviousPageToken();
            var response = await GetShoutsResponse(token);

            return response.Shouts.ToArray();
        }

        private async Task<ShoutsResponse> GetShoutsResponse(ContinuationToken token)
        {
            return await _httpClient.PostJsonAsync<ShoutsResponse>("api/Shouts/LatestShouts", token);
        }
    }
}
