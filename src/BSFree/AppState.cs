using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BSFree.Helpers;
using BSFree.Shared;
using Microsoft.AspNetCore.Blazor;

namespace BSFree
{
    public class AppState
    {
        private readonly HttpClient _httpClient;
        private readonly IPaginationHelper _paginationHelper;

        public IReadOnlyList<Shout> CurrentShoutsPage { get; private set; }
        public bool HasPreviousPage => _paginationHelper.HasPreviousPage;
        public bool HasNextPage => _paginationHelper.HasNextPage;
        public event Action OnChange;

        public AppState(HttpClient httpClient, IPaginationHelper paginationHelper)
        {
            _httpClient = httpClient;
            _paginationHelper = paginationHelper;
        }

        public async Task GetNextShoutsPage()
        {
            var token = _paginationHelper.GetNextPageToken();
            var response = await GetShoutsResponse(token);

            _paginationHelper.AddToken(response.ContinuationToken);

            CurrentShoutsPage = response.Shouts.ToArray();
            NotifyStateChanged();
        }

        public async Task GetPreviousShoutsPage()
        {
            var token = _paginationHelper.GetPreviousPageToken();
            var response = await GetShoutsResponse(token);

            CurrentShoutsPage = response.Shouts.ToArray();
            NotifyStateChanged();
        }

        private async Task<ShoutsResponse> GetShoutsResponse(ContinuationToken token)
        {
            return await _httpClient.PostJsonAsync<ShoutsResponse>("api/Shouts/LatestShouts", token);
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}