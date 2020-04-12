using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FloodGate.SDK.Tests
{
    [TestClass()]
    public class EvaluateTargetTests
    {
        private string sdkKey = "292b2f453a30c0f65c3414c73bb7e1ba2e42d1c02a2af1f7ada9f425187c";

        // Positive - Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "yellow")]
        public void ValidateFlagResponse_TargetEqualTo_ShouldReturnYellow(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "name", "Peter Parker" }, // left for backwards compatability testing
                { "country", "US" }, // left for backwards compatability testing
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "spiderman@marvel.com",
                Name = "Peter Parker",
                // Country = "US",
                // CustomAttributes = customAttributes
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Not Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "orange")]
        public void ValidateFlagResponse_TargetNotEqualTo_ShouldReturnOrange(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Name = "Bruce Banner",
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Greater Than
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "blue")]
        public void ValidateFlagResponse_TargetGreaterThan_ShouldReturnBlue(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "score1", "45" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Greater Than and Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "green")]
        public void ValidateFlagResponse_TargetGreaterThanEqualTo_ShouldReturnGreen(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Score2", "135" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Less Than
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "yellow")]
        public void ValidateFlagResponse_TargetLessThan_ShouldReturnYellow(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Score3", "35" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Less Than and Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "blue")]
        public void ValidateFlagResponse_TargetLessThanEqualTo_ShouldReturnBlue(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Score4", "120" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Contains
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "green")]
        public void ValidateFlagResponse_TargetContains_ShouldReturnGreen(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "random@gmail.com"
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Does not Contain
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "yellow")]
        public void ValidateFlagResponse_TargetNotContains_ShouldReturnYellow(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "email@yahoo.co.uk"
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Positive - Ends With
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "yellow")]
        public void ValidateFlagResponse_TargetEndsWith_ShouldReturnYellow(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                // { "name", "Peter Parker" }, // left for backwards compatability testing
                { "Country", "United Kingdom" }, // left for backwards compatability testing
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }





        // Negative - Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetEqualTo_ShouldReturnDefault(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b");

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Not Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetNotEqualTo_ShouldReturnDefault(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Name = "Thor",
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Greater Than
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetGreaterThan_ShouldReturnDefault(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "score1", "0" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Greater Than and Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetGreaterThanEqualTo_ShouldReturnDefault(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Score2", "0" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Less Than
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetLessThan_ShouldReturnDefault(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Score3", "350" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Less Than and Equal To
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetLessThanEqualTo_ShouldReturnDefault(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Score4", "1200" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Contains
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetContains_ShouldReturnDefault(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b");

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Does not Contain
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetNotContains_ShouldReturnDefault(string configFile, string expected)
        {
            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
                Email = "email@yahoo.com"
            };

            AutoUpdateClientConfig config = new AutoUpdateClientConfig()
            {
                SdkKey = sdkKey,
                ConfigFile = configFile,
                DisableCache = true
            };

            var floodGateClient = new FloodGateClient(config);

            var defaultValue = "grey";

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }

        // Negative - Ends With
        [DataTestMethod]
        [DataRow(@"..\..\..\test-config.json", "red")]
        public void ValidateFlagResponse_TargetEndsWith_ShouldReturnDefault(string configFile, string expected)
        {
            var customAttributes = new Dictionary<string, string>() {
                { "Country", "United States" }
            };

            User user = new User("d2405fc0-c9cd-49e7-a07e-bf244d6d360b")
            {
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

            var result = floodGateClient.GetValue("colours", defaultValue, user);

            Assert.AreEqual(expected, result);

            floodGateClient.Dispose();
        }
    }
}
