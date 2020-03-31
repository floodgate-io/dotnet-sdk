using FloodGate.SDK.Events;
using System;
using System.Threading;

namespace FloodGate.SDK
{
    public class AutoUpdateClientConfig : ClientConfigBase, IClientConfig, IDisposable
    {
        /// <summary>
        /// Interval in seconds for the client to refresh the data from the server
        /// </summary>
        public int RefreshInterval { get; set; } = 60;

        private Timer timer;

        public override void InitializeConfig(IHttpResourceFetcher httpResourceFetcher)
        {
            base.InitializeConfig(httpResourceFetcher);

            if (RefreshInterval <= 0)
            {
                throw new ArgumentOutOfRangeException("RefreshInterval must me greater than 0 seconds");
            }

            Logger.Debug($"RefreshInterval set to {RefreshInterval} seconds");

            timer = new Timer(AutoRefreshCallback, null, RefreshInterval * 1000, RefreshInterval * 1000);
        }

        private void AutoRefreshCallback(object sender)
        {
            if (!string.IsNullOrWhiteSpace(ConfigFile))
            {
                FetchFlagsLocally();

                return;
            }

            FetchFlagsServerAsync().Wait();
        }

        public new void Dispose()
        {
            //base.Dispose();

            timer?.Dispose();
        }
    }
}
