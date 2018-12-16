using BSFree.Services.Interfaces;
using BSFree.Shared;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSFree.Services
{
    public class ShoutsClient : IShoutsClient
    {
        private const string ApiUrl = "https://bsfree-api.azurewebsites.net/api/get-posts";

        private readonly IHttpClient _httpClient;
        private readonly string _apiKey;

        public ShoutsClient(IHttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<ShoutsResponse> GetLatestShouts(ContinuationToken continuationToken = null)
        {
            var apiUrl = $"{ApiUrl}?code={_apiKey}";

            var jsonContent = JsonConvert.SerializeObject(new
            {
                continuationToken
            });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            return await ReadAsAsync<ShoutsResponse>(response.Content);
        }

        private async Task<T> ReadAsAsync<T>(HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }
    }
}
