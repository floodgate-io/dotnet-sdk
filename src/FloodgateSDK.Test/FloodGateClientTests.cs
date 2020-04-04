using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FloodGate.SDK.Tests
{
    [TestClass()]
    public class FloodGateClientTests
    {
        private string sdkKey = "74c397533fbaece0401a0ff34dc055035779b242";

        // private string localConfigFileRolloutTarget = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\test-flags-rollout-target.json");
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

        // Default
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

        [TestMethod()]
        public void ValidateDefaultFlagResponse_ShouldReturnPink()
        {
            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = localConfigFileRolloutTarget
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("background-colour", defaultValue);

            Assert.AreEqual("pink", result);

            floodGateClient.Dispose();
        }


        // Target Tests
        [DataTestMethod]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "yellow")]
        [DataRow(@"..\..\..\test-flags-target.json", "yellow")]
        public void ValidateFlagResponse_Target1_ShouldReturnValid(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "name", "Peter Parker" },
                { "country", "US" },
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "spiderman@marvel.com",
                Name = "Peter Parker",
                Country = "US",
                CustomAttributes = customAttributes
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("background-colour", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        [DataTestMethod]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "orange")]
        [DataRow(@"..\..\..\test-flags-target.json", "orange")]
        public void ValidateFlagResponse_Target2_ShouldReturnValid(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "name", "Peter Parker" },
                { "country", "UK" },
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "spiderman@marvel.com",
                Name = "Peter Parker",
                Country = "UK",
                CustomAttributes = customAttributes
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("background-colour", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        [DataTestMethod]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "brown")]
        [DataRow(@"..\..\..\test-flags-target.json", "brown")]
        public void ValidateFlagResponse_Target3_ShouldReturnValid(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "spiderman@marvel.com"
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("background-colour", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Rollout Tests
        [DataTestMethod]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "d2405fc0-c9cd-49e7-a07e-bf244d6d360b", "blue")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "4d5817c5-4450-4cd4-b035-e24c2b72d50a", "purple")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "1dd97efd-62eb-418c-b2fe-11dd0ca188e0", "red")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "cb3c2d9c-9908-4f80-b7a4-c3cf2aa4d134", "yellow")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "da50fc56-16b5-4fbd-960a-89405968e881", "green")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "1527d16f-f287-49ba-bc61-bf86d39a2ab2", "brown")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "9daaed80-c889-457f-86b8-788d53a88b58", "pink")]
        [DataRow(@"..\..\..\test-flags-rollout-target.json", "2323e039-5f77-4391-b478-0b04f6541094", "orange")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "d2405fc0-c9cd-49e7-a07e-bf244d6d360b", "blue")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "4d5817c5-4450-4cd4-b035-e24c2b72d50a", "purple")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "1dd97efd-62eb-418c-b2fe-11dd0ca188e0", "red")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "cb3c2d9c-9908-4f80-b7a4-c3cf2aa4d134", "yellow")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "da50fc56-16b5-4fbd-960a-89405968e881", "green")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "1527d16f-f287-49ba-bc61-bf86d39a2ab2", "brown")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "9daaed80-c889-457f-86b8-788d53a88b58", "pink")]
        [DataRow(@"..\..\..\test-flags-rollout.json", "2323e039-5f77-4391-b478-0b04f6541094", "orange")]
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

            var result = floodGateClient.GetValue("background-colour", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }
    }
}
