using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FloodGate.SDK.Tests
{
    [TestClass()]
    public class EvaluateRolloutTests
    {
        private string sdkKey = "292b2f453a30c0f65c3414c73bb7e1ba2e42d1c02a2af1f7ada9f425187c";

        // Rollout Tests
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "1fa1cc72-6463-47d8-87df-a05a4e832dba", "red")]
        [DataRow(@"..\..\..\test-config.json", "4d5817c5-4450-4cd4-b035-e24c2b72d50a", "green")]
        [DataRow(@"..\..\..\test-config.json", "1dd97efd-62eb-418c-b2fe-11dd0ca188e0", "yellow")]
        [DataRow(@"..\..\..\test-config.json", "da50fc56-16b5-4fbd-960a-89405968e881", "orange")]
        [DataRow(@"..\..\..\test-config.json", "cb3c2d9c-9908-4f80-b7a4-c3cf2aa4d134", "blue")]
        public void ValidateFlagResponse_Rollout_ShouldReturnValid(string configFile, string userId, string expected)
        {
            User user = new User(userId);

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("rollout-colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }
    }
}
