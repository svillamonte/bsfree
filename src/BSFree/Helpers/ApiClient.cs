using System.Net.Http;
using System.Threading.Tasks;
using BSFree.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BSFree.Helpers
{
    public interface IApiClient
    {
        Task<ShoutsResponse> GetShoutsResponse(ContinuationToken token);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ShoutsResponse> GetShoutsResponse(ContinuationToken token)
        {
            return await _httpClient.PostJsonAsync<ShoutsResponse>("api/Shouts/LatestShouts", token);
        }
    }
}
