using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BSFree.Helpers;
using BSFree.Shared;
using Microsoft.AspNetCore.Blazor;
using Microsoft.JSInterop;

namespace BSFree
{
    public class AppState
    {
        private readonly IPaginationService _paginationService;

        public IReadOnlyList<Shout> CurrentShoutsPage { get; private set; } = new Shout[0];
        public bool IsLoading { get; private set; } = true;
        public bool HasPreviousPage => _paginationService.HasPreviousPage;
        public bool HasNextPage => _paginationService.HasNextPage;
        public bool AnyContent => CurrentShoutsPage.Any();
        public event Action OnChange;

        public AppState(IPaginationService paginationService) =>
            _paginationService = paginationService;

        [JSInvokable]
        public async Task GetNextShoutsPage()
        {
            // Fetched at least a page and no more after it
            if (CurrentShoutsPage.Any() && !HasNextPage)
            {
                return;
            }

            IsLoading = true;
            NotifyStateChanged();

            CurrentShoutsPage = await _paginationService.GetNextShoutsPage();
            IsLoading = false;
            NotifyStateChanged();

            await ScrollToTop();
        }

        public async Task GetPreviousShoutsPage()
        {
            IsLoading = true;
            NotifyStateChanged();

            CurrentShoutsPage = await _paginationService.GetPreviousShoutsPage();
            IsLoading = false;
            NotifyStateChanged();

            await ScrollToTop();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        private async Task ScrollToTop()
        {
            await JSRuntime.Current.InvokeAsync<string>("scrollToTop");
        }
    }
}