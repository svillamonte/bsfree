using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BSFree.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BSFree
{
    public class AppState
    {
        private readonly HttpClient _httpClient;

        public IReadOnlyList<Shout> CurrentShoutsPage { get; private set; }
        public ContinuationToken ContinuationToken { get; private set; }

        public event Action OnChange;

        public AppState(HttpClient httpClient) => _httpClient = httpClient;

        public async Task GetNextShoutsPage()
        {
            var response = await _httpClient.PostJsonAsync<ShoutsResponse>("api/Shouts/LatestShouts", ContinuationToken);

            CurrentShoutsPage = response.Shouts.ToArray();
            ContinuationToken = response.ContinuationToken;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}