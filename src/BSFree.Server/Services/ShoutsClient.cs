using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BSFree.Shared;
using Newtonsoft.Json;

namespace BSFree.Server.Services
{
    public class ShoutsClient
    {
        private const string ApiUrl = "https://bsfree-api.azurewebsites.net/api/get-posts";
        private readonly IHttpClient _httpClient;
        private readonly string _apiKey;

        public ShoutsClient(IHttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<ShoutsResponse> GetShouts()
        {
            var apiUrl = $"{ApiUrl}?code={_apiKey}";

            var jsonContent = JsonConvert.SerializeObject(new
            {
                body = new
                {
                    continuationToken = ""
                }
            });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
            return await response.Content.ReadAsAsync<ShoutsResponse>();
        }
    }


}
