using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FloodGate.SDK.Tests
{
    [TestClass()]
    public class HttpRsourceTests
    {
        private string sdkKey = "927f4418d15e9a81c834dcfe1c9f3d91d994cf9aff9813fb75b94f2e44f5";

        [TestMethod()]
        public void CreateAutoUpdateInstanceWithRemoteConfig_ShouldReturnValid()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey
            };

            var floodGateClient = new FloodGateClient(config);

            Assert.IsInstanceOfType(floodGateClient, typeof(FloodGateClient));

            floodGateClient.Dispose();
        }
    }
}
