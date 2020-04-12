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
                // SdkKey = "89fd83c921b59e720f939062b42770214e8aca6b80d749060a22c8a5b8a5",
                SdkKey = "2d414c28563b9788a47e5062fe9a57fd86efe69c28a31ea4aaada43cac87",
                Timeout = 5000,
                //Logger = new FileLogger(@"C:\Temp\FloodGateASPLog.txt"),
                RefreshInterval = 5, // RefreshInterval is set to 60 seconds by default, you can override it as needed
                //ConfigUrl = "http://localhost:8765", // Used to overwrite production URL
                //EventsUrl = "http://localhost:3000"
                // ConfigFile = @"c:\temp\flags.json"
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