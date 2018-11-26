using System.Net.Http;
using System.Threading.Tasks;

namespace BSFree.Server.Services
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}