using System;
using FloodGate.SDK;

namespace WebApplication_Framework.Services
{
    public class FloodGateWrapper
    {
        private static readonly Lazy<FloodGateWrapper> instance = new Lazy<FloodGateWrapper>(() => new FloodGateWrapper());

        public static FloodGateWrapper Instance { get { return instance.Value; } }

        public FloodGateClient Client;

        private FloodGateWrapper()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = "927f4418d15e9a81c834dcfe1c9f3d91d994cf9aff9813fb75b94f2e44f5",
                Timeout = 10000,
                Logger = new FileLogger(@"C:\Temp\FloodGateASPLog.txt"),
                RefreshInterval = 5
            };

            try
            {
                Client = new FloodGateClient(config);
            }
            catch (Exception exception)
            {
                // Handle error here...
                throw exception;
            }
        }
    }
}