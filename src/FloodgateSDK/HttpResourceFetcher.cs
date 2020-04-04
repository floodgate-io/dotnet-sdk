using FloodGate.SDK.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    internal sealed class HttpResourceFetcher : IHttpResourceFetcher
    {
        private readonly ILogger logger;

        private HttpClient httpClient = new HttpClient();

        public HttpResourceFetcher(ILogger logger)
        {
            this.logger = logger;

            httpClient.DefaultRequestHeaders.Add("X-FloodGate-SDK-Agent", "dotnet");
            httpClient.DefaultRequestHeaders.Add("X-FloodGate-SDK-Version", ClientConfigBase.AssemblyVersion);
        }

        public async Task<string> FetchAsync(Uri requestUri, IClientConfig config, string sdkKey, CancellationToken token = default(CancellationToken))
        {
            string result = null;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = requestUri
                };

                if (config.ETag != null)
                {
                    request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(config.ETag));
                }

                using (var response = await httpClient.SendAsync(request, token).ConfigureAwait(false))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
                    {
                        logger.Info($"CDN not modified");
                        return result; // Just return empty result, flags will be loaded from the cache
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        logger.Warning("Check your SDK Key");
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        config.ETag = response.Headers.ETag?.ToString() ?? null;
                        logger.Info($"ETag = {config.ETag}");
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Debug(exception.Message);
            }

            return result;
        }
    }
}
