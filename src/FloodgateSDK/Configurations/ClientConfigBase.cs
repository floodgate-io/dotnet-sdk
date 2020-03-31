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
        public int Timeout { get; set; } = 10000;

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
		public List<FeatureFlagEntity> Flags { get; private set; } = new List<FeatureFlagEntity>();

        /// <summary>
        /// The default state returned if a flag is not found
        /// </summary>
        public bool DefaultFlagState { get; set; } = false;

        public string ConfigUrl { get; set; } = string.Empty;

        public string ConfigFile { get; set; } = string.Empty;

        /// <summary>
        /// User assigned at config level, this user is overwritten if passed in at the evaluation stage
        /// </summary>
        private User user;

        //public IEventProcessor EventProcessor { get; private set; }

        public IHttpResourceFetcher HttpResourceFetcher { get; private set; }

        internal const string API_VERSION = "v1";

        public ClientConfigBase()
        {
            
        }

        public virtual void InitializeConfig(IHttpResourceFetcher httpResourceFetcher)
        {
            //EventProcessor = new EventProcessor(Logger);
            // EventProcessor = eventProcessor;

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

        protected async Task LoadData()
        {
            // Load from cache
            if (!DisableCache && Cache.Exists(Consts.CACHE_NAME))
            {
                Logger.Debug("Loading config from cache");

                // TODO: add timeout check
                var json = Cache.Retrieve<string>(Consts.CACHE_NAME);

                if (ValidateJson(json))
                {
                    DeserializeConfigJson(json);

                    isLoaded = true;

                    return;
                }
            }

            // Load from local file
            if (!string.IsNullOrWhiteSpace(ConfigFile))
            {
                // If the file cannot be loaded continue to load from server
                // Note this is not an async call and is blocking
                if (FetchFlagsLocally())
                {
                    isLoaded = true;

                    return;
                }
            }

            // Load from CDN
            var task = FetchFlagsServerAsync();
            if (await Task.WhenAny(task, Task.Delay(Timeout)).ConfigureAwait(false) == task)
            {
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

            // HttpResourceFetcher httpResourceFetcher = new HttpResourceFetcher(Logger);

            var json = await HttpResourceFetcher.FetchAsync(BuildUrl("flags"), string.Empty, SdkKey).ConfigureAwait(false);

            if (ValidateJson(json))
            {
                DeserializeConfigJson(json);

                Cache.Save<string>(Consts.CACHE_NAME, json);
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
                        DeserializeConfigJson(json);

                        Cache.Save<string>(Consts.CACHE_NAME, json);

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

        /// <summary>
        /// Deserialize the json config data to objects
        /// </summary>
        /// <param name="json"></param>
        private void DeserializeConfigJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ApplicationException("Config data empty");
            }

            try
            {
                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                var deserializer = new DataContractJsonSerializer(Flags.GetType());

                Flags = deserializer.ReadObject(memoryStream) as List<FeatureFlagEntity>;

                memoryStream.Close();

                if (Flags == null || Flags.Count == 0)
                {
                    throw new ApplicationException("Empty or invalid server response.");
                }

                Logger.Debug($"Loaded {Flags.Count} flags");
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
