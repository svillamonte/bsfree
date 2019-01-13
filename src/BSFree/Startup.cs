using BSFree.Helpers;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BSFree
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContinuationTokenHelper, ContinuationTokenHelper>();
            services.AddScoped<IApiClient, ApiClient>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddSingleton<AppState>();
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
