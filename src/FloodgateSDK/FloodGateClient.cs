using System;
using System.Reflection;
using System.Linq;
using FloodGate.SDK.Evaluators;

namespace FloodGate.SDK
{
    /// <summary>
    /// Client for floodgate.io
    /// </summary>
    public class FloodGateClient : IFloodGateClient
	{
		private ILogger log;

		private IClientConfig config;

		private static readonly string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();


		/// <summary>
		/// Create a new instance of FloodGate client and setup default configuration
		/// </summary>
		/// <param name="sdkKey">Floodgate environment SDK Key</param>
        /// <exception cref="TimeoutException">Throws timeout exception on configuration load timeout</exception>
		public FloodGateClient(string sdkKey) : this(new AutoUpdateClientConfig { SdkKey = sdkKey, Cache = new InMemoryStoreCache() })
		{
		}

		/// <summary>
		/// Create a new instance of the FloodGate client based on a custom configuration
		/// </summary>
		/// <param name="config"></param>
		public FloodGateClient(AutoUpdateClientConfig config)
		{
			try
			{
                config.ValidateConfig();

                config.InitializeConfig();
                
                InitializeClient(config);
			}
			catch(Exception exception)
			{
				throw new ApplicationException("FloodGate AutoUpdateClientConfig failed to load", exception);
			}
		}

        public FloodGateClient(DefaultClientConfig config)
        {
            try
            {
                config.ValidateConfig();
                
                config.InitializeConfig();

                InitializeClient(config);
            }
            catch (Exception exception)
            {
                throw new ApplicationException("FloodGate DefaultClientConfig failed to load", exception);
            }
        }

        private void InitializeClient(IClientConfig configuration)
		{
            config = configuration;

            log = config.Logger;

            log.Debug($"Version: {version}");

            log.Debug($"SdkApiUrl: {config.BuildUrl("flags")?.ToString()}");
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
                // Check to see if there is a user set at the config level
                User user = config.GetUser();

                // Override the user if one if passed
                if (overrideUser != null)
                {
                    log.Info($"Overriding user");

                    user = overrideUser;
                }

                // If there is no flag data present at all, return the default value
                if (config.Flags.ToList().Count == 0)
                {
                    log.Error("No flag data available");

                    return defaultValue;
                }

                FeatureFlagEntity flag = config.Flags.Where(q => q.Key == key).SingleOrDefault();

                // If no flag is found, return the default value
                if (flag == null)
                {
                    log.Info($"{key} not found");

                    return defaultValue;
                }

                // If user is null then cannot evaluate targets or rollouts, return the flag default value
                if (user == null)
                {
                    log.Info($"{flag.Id}, {flag.Value}");
                    
                    return (T)Convert.ChangeType(flag.Value, typeof(T));
                }

                // If targeting not enabled, try and evaluate rollouts
                if (!flag.IsTargetingEnabled)
                {
                    log.Info($"{flag.Id}, {flag.Value}");

                    // Evaluate percentage rollouts
                    if (flag.IsRollout)
                    {
                        return RolloutEvaluator.Evaluate<T>(key, user.Id, flag.Rollouts, (T)Convert.ChangeType(flag.Value, typeof(T)), log); 
                    }

                    return (T)Convert.ChangeType(flag.Value, typeof(T));
                }

                // If targeting is enabled and there are targets present, evaluate the targets
                if (flag.Targets.Count > 0)
                {
                    log.Info("Evaluating targets");
                    return TargetEvaluator.Evaluate<T>(key, user, flag, (T)Convert.ChangeType(flag.Value, typeof(T)), log);
                }
            }
            catch (Exception exception)
            {
                log.Error(exception);

                throw;
            }

            return defaultValue;
        }

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

        public void Dispose()
        {
            if (config != null && config is IDisposable)
            {
                ((IDisposable)config).Dispose();
            }
        }
    }
}
