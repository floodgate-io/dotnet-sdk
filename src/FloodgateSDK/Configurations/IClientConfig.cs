﻿using FloodGate.SDK.Events;
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

        /// <summary>
        /// Url for overriding the default location for sending events
        /// </summary>
        string EventsUrl { get; set; }

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

        List<FeatureFlagEntity> GetFlags();

        string RawConfigData { get; set; }

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
        void InitializeConfig(IHttpResourceFetcher httpResourceFetcher);

        /// <summary>
        /// Build Uri for communicating to floodgate.io Api servers
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        Uri BuildUrl(string endpoint);

        /// <summary>
        /// When will the current cache expire
        /// </summary>
        //int CacheExpiry { get; set; }

        //bool IsCacheExpired { get; }

        /// <summary>
        /// Check to see if the config needs to be refreshed
        /// </summary>
        //void Refresh();

        string ETag { get; set; }

        /// <summary>
        /// The time the data was fetched from the server
        /// </summary>
        //DateTime FetchedTime { get; set; }

        /// <summary>
        /// Force refresh of the configuration
        /// </summary>
        void Refresh();
    }
}
