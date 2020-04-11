using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FloodGate.SDK.Tests
{
    [TestClass()]
    public class FloodGateClientTests
    {
        private string sdkKey = "292b2f453a30c0f65c3414c73bb7e1ba2e42d1c02a2af1f7ada9f425187c";
        private string localConfigFileRolloutTarget = @"..\..\..\test-flags-rollout-target.json";

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod()]
        public void CreateInstanceWithEmptySdkKey_ShouldReturnException()
        {
            System.Diagnostics.Debug.WriteLine("CreateInstanceWithEmptySdkKey_ShouldReturnException");

            new FloodGateClient(string.Empty);
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod()]
        public void CreateAutoUpdateInstanceWithEmptySdkKey_ShouldReturnException()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = string.Empty
            };

            new FloodGateClient(config);
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod()]
        public void CreateAutoUpdateInstanceWithInvalidRefreshInterval_ShouldReturnException()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                RefreshInterval = 0
            };

            new FloodGateClient(config);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void CreateAutoUpdateInstanceWithInvalidUserObjectEmptyString_ShouldReturnException()
        {
            User user = new User("");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void CreateAutoUpdateInstanceWithInvalidUserObjectNull_ShouldReturnException()
        {
            User user = new User(null);
        }

        [TestMethod()]
        public void CreateAutoUpdateInstanceWithInvalidLocalConfig_ShouldReturnValid()
        {
            var file = @"test-flags.json";
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = file
            };

            var floodGateClient = new FloodGateClient(config);

            Assert.IsInstanceOfType(floodGateClient, typeof(FloodGateClient));

            floodGateClient.Dispose();
        }

        [TestMethod()]
        public void CreateAutoUpdateInstanceWithLocalConfig_ShouldReturnValid()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = localConfigFileRolloutTarget
            };

            var floodGateClient = new FloodGateClient(config);

            Assert.IsInstanceOfType(floodGateClient, typeof(FloodGateClient));

            floodGateClient.Dispose();
        }

        


        

        
    }
}
