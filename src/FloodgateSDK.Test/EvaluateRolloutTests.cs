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
        [DataRow(@"..\..\..\test-config.json", "a9ac317e-6510-4903-9a42-e43d775b816d", "red")]
        [DataRow(@"..\..\..\test-config.json", "a8ca3305-d4fc-4865-ae75-72edb8949244", "green")]
        [DataRow(@"..\..\..\test-config.json", "ec144edb-7102-4091-ae1a-0d7b5f74707a", "yellow")]
        [DataRow(@"..\..\..\test-config.json", "1fa1cc72-6463-47d8-87df-a05a4e832dba", "orange")]
        [DataRow(@"..\..\..\test-config.json", "7af3cd58-dc5b-4d53-982b-4892fd9b7df2", "blue")]
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
