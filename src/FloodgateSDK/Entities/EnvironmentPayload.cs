namespace FloodGate.SDK
{
    public class EnvironmentPayload
    {
        /// <summary>
        /// ETag header for CDN data
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// Raw Json data returned from CDN
        /// </summary>
        public string Json { get; set; }
    }
}
