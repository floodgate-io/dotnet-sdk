using System;

namespace FloodGate.SDK
{
    public interface IFloodGateClient : IDisposable
    {
        /// <summary>
        /// Returns value of flag for the given key
        /// </summary>
        /// <param name="key">The flag key to check</param>
        /// <param name="user">Optional: A user to compare against when flag targeting is enabled</param>
        /// <returns>String value of the flag. If no flag data is found for the key given the default string of `False` is returned</returns>
        string GetValue(string key, User user = null);

        /// <summary>
        /// Returns value of flag for the given key
        /// </summary>
        /// <param name="key">The flag key to check</param>
        /// <param name="defaultValue">A default value to return if the no flag data is found for the key given</param>
        /// <param name="user">Optional: A user to compare against when flag targeting is enabled</param>
        /// <returns>Boolean value of the flag</returns>
        bool GetValue(string key, bool defaultValue, User user = null);

        /// <summary>
        /// Returns value of flag for the given key
        /// </summary>
        /// <param name="key">The flag key to check</param>
        /// <param name="defaultValue">A default value to return if the no flag data is found for the key given</param>
        /// <param name="user">Optional: A user to compare against when flag targeting is enabled</param>
        /// <returns>String value of the flag</returns>
        string GetValue(string key, string defaultValue, User user = null);

        /// <summary>
        /// Returns value of flag for the given key
        /// </summary>
        /// <param name="key">The flag key to check</param>
        /// <param name="defaultValue">A default value to return if the no flag data is found for the key given</param>
        /// <param name="user">Optional: A user to compare against when flag targeting is enabled</param>
        /// <returns>Integer value of the flag</returns>
        int GetValue(string key, int defaultValue, User user = null);

        /// <summary>
        /// Returns value of flag for the given key
        /// </summary>
        /// <param name="key">The flag key to check</param>
        /// <param name="defaultValue">A default value to return if the no flag data is found for the key given</param>
        /// <param name="user">Optional: A user to compare against when flag targeting is enabled</param>
        /// <returns>Double value of the flag</returns>
        double GetValue(string key, double defaultValue, User user = null);
    }
}
