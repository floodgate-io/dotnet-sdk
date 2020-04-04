using FloodGate.SDK.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace FloodGate.SDK
{
    public abstract class ClientConfigBase : IClientConfig, IDisposable
    {
        internal static string AssemblyVersion = typeof(FloodGateClient).Assembly.GetName().Version?.ToString();

        private bool isLoaded { get; set; }

        /// <summary>
        /// Timeout value for fetching data from CDN
        /// Value is in milliseconds
        /// </summary>
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// Sdk key used to get the configuration from api.floodgate.io
        /// </summary>
        public string SdkKey { get; set; }

        /// <summary>
        /// Base url for floodgate.io Api servers
        /// </summary>
        private const string API_BASE_URL = "https://cdn.floodgate.io";

        /// <summary>
        /// Logger for logging client activity
        /// </summary>
        public ILogger Logger { get; set; } = new DefaultLogger();

        /// <summary>
        /// Cache object used for caching data locally
        /// </summary>
        public ICache Cache { get; set; } = new InMemoryStoreCache();

        /// <summary>
        /// Disable caching
        /// </summary>
        public bool DisableCache { get; set; } = false;

        /// <summary>
		/// List containing FeatureFlag entities for the current environment
		/// </summary>
		public List<FeatureFlagEntity> Flags { get; set; } = new List<FeatureFlagEntity>();

        /// <summary>
        /// The default state returned if a flag is not found
        /// </summary>
        public bool DefaultFlagState { get; set; } = false;

        public string ConfigUrl { get; set; } = string.Empty;

        public string ConfigFile { get; set; } = string.Empty;

        public string EventsUrl { get; set; } = string.Empty;

        /// <summary>
        /// Stored the current ETag of the loaded configuration
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// Store the raw flag json data
        /// </summary>
        public string RawConfigData { get; set; } = string.Empty;

        /// <summary>
        /// User assigned at config level, this user is overwritten if passed in at the evaluation stage
        /// </summary>
        private User user;

        /// <summary>
        /// When will the current cache expire
        /// </summary>
        //public int CacheExpiry { get; set; } = 3600; // Expire after 1 hour

        //public DateTime FetchedTime { get; set; } = DateTime.Now;

        //public bool IsCacheExpired { get { return DateTime.Now > FetchedTime.AddSeconds(CacheExpiry); } }

        public IHttpResourceFetcher HttpResourceFetcher { get; private set; }

        internal const string API_VERSION = "v1";

        public ClientConfigBase()
        {
            
        }

        public virtual void InitializeConfig(IHttpResourceFetcher httpResourceFetcher)
        {
            HttpResourceFetcher = httpResourceFetcher;

            LoadData().Wait();
        }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        /// <exception cref="ArgumentNullException">When the configuration contains an empty or null Sdk Key</exception>
        public void ValidateConfig()
        {
            if (string.IsNullOrEmpty(SdkKey))
            {
                Logger.Error("SDK key was empty");
                throw new ArgumentNullException("SdkKey", "SDK Key was empty");
            }
        }

        public User GetUser()
        {
            return user;
        }

        public void SetUser(User user)
        {
            this.user = user;
        }

        public void UnsetUser()
        {
            this.user = null;
        }

        /// <summary>
        /// Build Uri for communicating to floodgate.io Api servers
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public Uri BuildUrl(string endpoint)
        {
            if (!string.IsNullOrEmpty(ConfigUrl))
            {
                var url = $"{ConfigUrl}/environment-files/{SdkKey}/{API_VERSION}/flags-config.json";

                Uri uriResult;
                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result)
                    return new Uri(url);
            }

            return new Uri($"{API_BASE_URL}/environment-files/{SdkKey}/{API_VERSION}/flags-config.json");
        }

        public void Refresh()
        {
            LoadData().ConfigureAwait(false);
        }

        protected async Task LoadData()
        {
            // Load from local file
            if (!string.IsNullOrWhiteSpace(ConfigFile))
            {
                // If the file cannot be loaded continue to load from server
                // Note this is not an async call and is blocking
                if (FetchFlagsLocally())
                {
                    Logger.Debug("Loading config from local file");

                    isLoaded = true;

                    return;
                }
            }

            // Load from CDN
            Task task = FetchFlagsServerAsync();
            if (await Task.WhenAny(task, Task.Delay(Timeout)).ConfigureAwait(false) == task)
            {
                Logger.Debug("Loading config from server");

                isLoaded = true;

                return;
            }
            else
            {
                // Timeout
                isLoaded = false;

                throw new TimeoutException("Configutation data failed to load because of CDN timeout");
            }
        }

        /// <summary>
        /// Fetch config data from server
        /// </summary>
        protected async Task FetchFlagsServerAsync()
        {
            Logger.Info("Requesting flag data from server");

            var json = await HttpResourceFetcher.FetchAsync(BuildUrl("flags"), this, SdkKey).ConfigureAwait(false);

            if (ValidateJson(json))
            {
                Cache.Save<string>(Consts.CACHE_NAME, json);

                Logger.Info("*** Config Cached (remote) ***");
            }
        }

        protected bool FetchFlagsLocally()
        {
            Logger.Info("Loading flag data from local file");

            try
            {
                using (StreamReader r = new StreamReader(ConfigFile))
                {
                    string json = r.ReadToEnd();

                    if (ValidateJson(json))
                    {
                        Cache.Save<string>(Consts.CACHE_NAME, json);

                        Logger.Info("*** Config Cached (local) ***");

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Validate the Json response returned from the server
        /// </summary>
        private bool ValidateJson(string json)
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(json))
                return false;

            if (json == "[]")
                return false;

            if (json == "null")
                return false;

            return true;
        }

        public List<FeatureFlagEntity> GetFlags()
        {
            var json = Cache.Retrieve<string>(Consts.CACHE_NAME);
            if (ValidateJson(json))
            {
                Logger.Info("Getting flags from cache");

                return DeserializeConfigJson(json);
            }

            return new List<FeatureFlagEntity>();
        }

        /// <summary>
        /// Deserialize the json config data to objects
        /// </summary>
        /// <param name="json"></param>
        private List<FeatureFlagEntity> DeserializeConfigJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ApplicationException("Config data empty");
            }

            try
            {
                Logger.Info("Deserializing Config Json");

                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                var deserializer = new DataContractJsonSerializer(typeof(List<FeatureFlagEntity>));

                var flags = deserializer.ReadObject(memoryStream) as List<FeatureFlagEntity>;

                memoryStream.Close();

                memoryStream.Dispose();

                if (flags == null || flags.Count == 0)
                {
                    throw new ApplicationException("Empty or invalid flag configuration data");
                }

                Logger.Debug($"Loaded {Flags.Count} flags");

                return flags;
            }
            catch (Exception exception)
            {
                Logger.Error(exception);

                throw;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //public void FlushEvents()
        //{
        //    EventProcessor.ManualFlush();
        //}

        //public void Dispose()
        //{
        //    EventProcessor.Dispose();
        //}
    }
}
