namespace FloodGate.SDK
{
    public interface ICache
    {
        void Initialize();

        /// <summary>
        /// Chck to see if an item in the cache exists 
        /// </summary>
        /// <returns>Returns true if cache item exists</returns>
        bool Exists(string name);

        /// <summary>
        /// Retrieve the current data from the cache
        /// </summary>
        /// <returns>Boolean</returns>
        T Retrieve<T>(string name);

        /// <summary>
        /// Save data to the cache
        /// </summary>
        void Save<T>(string name, string json);
    }
}
