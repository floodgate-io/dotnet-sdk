using System;
using System.Reflection;
using System.Linq;
using FloodGate.SDK.Evaluators;
using FloodGate.SDK.Events;

namespace FloodGate.SDK
{
    /// <summary>
    /// Client for floodgate.io
    /// </summary>
    public class FloodGateClient : IFloodGateClient
	{
		private ILogger logger;

		private IClientConfig config;

        private IEventProcessor eventProcessor;

        private HttpResourceFetcher httpResourceFetcher;

		private static readonly string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();


		/// <summary>
		/// Create a new instance of FloodGate client and setup default configuration
		/// </summary>
		/// <param name="sdkKey">Floodgate environment SDK Key</param>
        /// <exception cref="TimeoutException">Throws timeout exception on configuration load timeout</exception>
		public FloodGateClient(string sdkKey) : this(new AutoUpdateClientConfig { SdkKey = sdkKey, Cache = new InMemoryStoreCache(), Logger = new DefaultLogger() })
		{
            InitializeClient();
        }

        public FloodGateClient(IClientConfig config)
        {
            this.config = config;

            InitializeClient();
        }

        private void InitializeClient()
        {
            try
            {
                logger = config.Logger;

                eventProcessor = new EventProcessor(logger, new EventsConfig() { EventsUrl = config.EventsUrl } );

                httpResourceFetcher = new HttpResourceFetcher(logger);

                config.ValidateConfig();

                config.InitializeConfig(httpResourceFetcher);

                logger.Debug($"Version: {version}");

                logger.Debug($"SdkApiUrl: {config.BuildUrl("flags")?.ToString()}");
            }
            catch (Exception exception)
            {
                // TODO: Get the config type
                throw new ApplicationException("FloodGate config failed to load", exception);
            }
        }

        /// <summary>
        /// Returns a value of a given flag for the given type
        /// </summary>
        /// <typeparam name="T">Data type of the flag being returned</typeparam>
        /// <param name="key">The flag key to check</param>
        /// <param name="defaultValue">The default value to return if no flag is found</param>
        /// <param name="overrideUser">An optional user to compare against</param>
        /// <returns>Returns either flag value or default value if a default value is provided</returns>
        private T Evaluate<T>(string key, T defaultValue, User overrideUser = null)
        {
            try
            {
                //config.Refresh();

                // Check to see if there is a user set at the config level
                User user = config.GetUser();

                // Override the user if one if passed
                if (overrideUser != null)
                {
                    logger.Info($"Overriding user");

                    user = overrideUser;
                }

                var flags = config.GetFlags();

                // If there is no flag data present at all, return the default value
                // if (config.Flags.ToList().Count == 0)
                if (flags.Count == 0)
                {
                    logger.Error("No flag data available");

                    return defaultValue;
                }

                // FeatureFlagEntity flag = config.Flags.Where(q => q.Key == key).SingleOrDefault();
                FeatureFlagEntity flag = flags.Where(q => q.Key == key).SingleOrDefault();

                // If no flag is found, return the default value
                if (flag == null)
                {
                    logger.Info($"{key} not found");

                    eventProcessor.AddToQueue(new FlagNotFoundEvent(config.SdkKey, key, defaultValue.ToString()));

                    return defaultValue;
                }

                // If user is null then cannot evaluate targets or rollouts, return the flag default value
                if (user == null)
                {
                    logger.Info($"{flag.Id}, {flag.Value}");

                    eventProcessor.AddToQueue(new FlagEvaluationEvent(config.SdkKey, flag.Key, flag.Value.ToString()));

                    return (T)Convert.ChangeType(flag.Value, typeof(T));
                }

                // If targeting not enabled, try and evaluate rollouts
                if (!flag.IsTargetingEnabled)
                {
                    logger.Info($"{flag.Id}, {flag.Value}");

                    // Evaluate percentage rollouts
                    if (flag.IsRollout)
                    {
                        var rolloutResult = RolloutEvaluator.Evaluate<T>(key, user.Id, flag.Rollouts, (T)Convert.ChangeType(flag.Value, typeof(T)), logger);

                        eventProcessor.AddToQueue(new FlagEvaluationEvent(config.SdkKey, flag.Key, rolloutResult.ToString(), user));

                        return rolloutResult;
                    }

                    eventProcessor.AddToQueue(new FlagEvaluationEvent(config.SdkKey, flag.Key, flag.Value.ToString(), user));

                    return (T)Convert.ChangeType(flag.Value, typeof(T));
                }

                // If targeting is enabled and there are targets present, evaluate the targets
                if (flag.Targets.Count > 0)
                {
                    logger.Info("Evaluating targets");

                    var targetResult = TargetEvaluator.Evaluate<T>(key, user, flag, (T)Convert.ChangeType(flag.Value, typeof(T)), logger);

                    eventProcessor.AddToQueue(new FlagEvaluationEvent(config.SdkKey, flag.Key, targetResult.ToString(), user));

                    return targetResult;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                throw;
            }

            return defaultValue;
        }

        #region GetValue methods
        public string GetValue(string key, User user = null)
        {
            return Evaluate(key, config.DefaultFlagState.ToString(), user);
        }

        public bool GetValue(string key, bool defaultValue, User user = null)
        {
            return Evaluate(key, defaultValue, user);
        }

        public string GetValue(string key, string defaultValue, User user = null)
        {
            return Evaluate(key, defaultValue, user);
        }

        public int GetValue(string key, int defaultValue, User user = null)
        {
            return Evaluate(key, defaultValue, user);
        }

        public double GetValue(string key, double defaultValue, User user = null)
        {
            return Evaluate(key, defaultValue, user);
        }
        #endregion


        public void FlushEvents()
        {
            eventProcessor.ManualFlush();
        }

        public void Dispose()
        {
            if (logger != null && logger is IDisposable)
            {
                ((IDisposable)logger).Dispose();
            }

            if (eventProcessor != null && eventProcessor is IDisposable)
            {
                ((IDisposable)eventProcessor).Dispose();
            }

            if (config != null && config is IDisposable)
            {
                ((IDisposable)config).Dispose();
            }
        }
    }
}
