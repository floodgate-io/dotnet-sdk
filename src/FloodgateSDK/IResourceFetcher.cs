using System;
using System.Threading;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    interface IResourceFetcher
    {
        Task<string> FetchAsync(Uri requestUri, string currentConfig, CancellationToken token);

        //Task<EnvironmentPayload> FetchAsync(Uri requestUri, EnvironmentPayload currentEnvironmentPayload);
    }
}
