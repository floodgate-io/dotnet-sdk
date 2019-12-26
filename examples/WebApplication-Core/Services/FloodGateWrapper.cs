///
/// Example of how to create Floodgate SDK as a Singleton
///

using System;
using FloodGate.SDK;


namespace WebApplication_Core.Services
{
    public class FloodGateWrapper
    {
        private static readonly Lazy<FloodGateWrapper> instance = new Lazy<FloodGateWrapper>(() => new FloodGateWrapper());

        public static FloodGateWrapper Instance { get { return instance.Value; } }

        public IFloodGateClient Client;

        private FloodGateWrapper()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = "927f4418d15e9a81c834dcfe1c9f3d91d994cf9aff9813fb75b94f2e44f5",
                Timeout = 10000,
                Logger = new ConsoleLogger(),
                RefreshInterval = 5, // RefreshInterval is set to 60 seconds by default, you can override it as needed
                // ConfigUrl = "http://localhost:8765"
                ConfigFile = @"c:\temp\flags.json"
            };

            try
            {
                Client = new FloodGateClient(config);
            }
            catch (Exception exception)
            {
                // Handle any errors here
                throw exception;
            }
        }

    }
}
