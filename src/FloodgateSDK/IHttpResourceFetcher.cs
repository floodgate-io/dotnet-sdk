using System;
using System.Threading;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    public interface IHttpResourceFetcher
    {
        //Task<string> FetchAsync(Uri requestUri, string currentConfig, string sdkKey, CancellationToken token);
        //Task<string> FetchAsync(Uri requestUri, string currentConfig, string sdkKey, CancellationToken token = default(CancellationToken));
        Task<string> FetchAsync(Uri requestUri, IClientConfig config, string sdkKey, CancellationToken token = default(CancellationToken));
    }
}
