using System;
using System.Threading;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    interface IResourceFetcher
    {
        Task<string> FetchAsync(Uri requestUri, string currentConfig, string sdkKey, CancellationToken token);
    }
}
