using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace FloodGate.SDK.Tests
{
    [TestClass()]
    public class DefaultValueTests
    {
        private string sdkKey = "292b2f453a30c0f65c3414c73bb7e1ba2e42d1c02a2af1f7ada9f425187c";
        private string localConfigFileRolloutTarget = @"..\..\..\test-config.json";

        // Default value when no flag exists
        [TestMethod()]
        public void ValidateFlagSDKDefaultResponse_ShouldReturnGrey()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = localConfigFileRolloutTarget
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("non-existent-flag", defaultValue);

            Assert.AreEqual(defaultValue, result);

            floodGateClient.Dispose();
        }

        // Existing flag default value
        [TestMethod()]
        public void ValidateDefaultFlagResponse_ShouldReturnRed()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = localConfigFileRolloutTarget
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue);

            Assert.AreEqual("red", result);

            floodGateClient.Dispose();
        }
    }
}
