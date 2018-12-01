using BSFree.Shared;
using System.Threading.Tasks;

namespace BSFree.Services.Interfaces
{
    public interface IShoutsClient
    {
        Task<ShoutsResponse> GetLatestShouts();
    }
}