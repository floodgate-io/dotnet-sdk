using System;
using System.Collections.Generic;

namespace FloodGate.SDK
{
    public interface IClientConfig
    {
        int Timeout { get; set; }

        /// <summary>
        /// Sdk key used to get the configuration from api.floodgate.io
        /// </summary>
        string SdkKey { get; set; }

        /// <summary>
        /// Logger for logging client activity
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        /// Cache object used for caching data locally
        /// </summary>
        ICache Cache { get; set; }

        /// <summary>
        /// Url for overriding the default location of the config file
        /// </summary>
        string ConfigUrl { get; set; }

        /// <summary>
        /// File location for overriding the default location of the config file
        /// </summary>
        string ConfigFile { get; set; }

        User GetUser();

        /// <summary>
        /// User object containing specific properties to the current user
        /// </summary>
        void SetUser(User user);

        /// <summary>
        /// Unset the current user object
        /// </summary>
        void UnsetUser();

        /// <summary>
        /// List of feature flags for the current configuration
        /// </summary>
        List<FeatureFlagEntity> Flags { get; }

        /// <summary>
        /// The default state returned if a flag is not found
        /// </summary>
        bool DefaultFlagState { get; set; }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        /// <exception cref="ArgumentNullException">When the configuration contains an empty or null Sdk Key</exception>
        void ValidateConfig();

        /// <summary>
        /// Initialize the current configuration object
        /// </summary>
        void InitializeConfig();

        /// <summary>
        /// Build Uri for communicating to floodgate.io Api servers
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Uri BuildUrl(string endpoint);


        // bool IsConfigReady(int milliseconds);
    }
}
