using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    internal sealed class HttpResourceFetcher : IResourceFetcher
    {
        private readonly ILogger log;

        private HttpClient httpClient = new HttpClient();

        public HttpResourceFetcher(ILogger logger)
        {
            log = logger;

            httpClient.DefaultRequestHeaders.Add("X-FloodGate-SDK-Agent", "dotnet-v" + ClientConfigBase.AssemblyVersion);
        }

        public async Task<string> FetchAsync(Uri requestUri, string currentConfig, CancellationToken token = default(CancellationToken))
        {
            string result = null;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                
                using (var response = await httpClient.GetAsync(requestUri, token).ConfigureAwait(false))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotModified && !string.IsNullOrEmpty(currentConfig))
                    {
                        return currentConfig;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception exception)
            {
                log.Debug(exception.Message);
            }

            return result;
        }

        //public async Task<EnvironmentPayload> FetchAsync(Uri requestUri, EnvironmentPayload currentEnvironmentPayload)
        //{
        //    EnvironmentPayload result = new EnvironmentPayload();

        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = requestUri
        //    };

        //    if (currentEnvironmentPayload.ETag != null)
        //    {
        //        request.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(currentEnvironmentPayload.ETag));
        //    }

        //    HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);

        //    if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
        //    {
        //        return currentEnvironmentPayload;
        //    }

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var httpETag = response.Headers.ETag.Tag;

        //        result.Json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //    }

        //    return result;
        //}
    }
}
