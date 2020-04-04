using System;
using System.Runtime.Caching;

namespace FloodGate.SDK
{
    public class InMemoryStoreCache : ICache
    {
        public void Initialize()
        {
        }

        public bool Exists(string name)
        {
            ObjectCache cache = MemoryCache.Default;

            return cache.Contains(name);
        }

        public T Retrieve<T>(string name)
        {
            ObjectCache cache = MemoryCache.Default;
            
            var cacheObject = (T)cache[name];

            return cacheObject;
        }

        public void Save<T>(string name, string json)
        {
            SaveObjectToCache<string>(name, json);
        }

        private void SaveObjectToCache<T>(string cacheName, T objectToCache)
        {
            ObjectCache cache = MemoryCache.Default;

            // TODO: Update expirt based on refresh time
            CacheItemPolicy policy = new CacheItemPolicy();

            policy.Priority = CacheItemPriority.NotRemovable;
            // policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10);
            // policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_cacheSeconds);

            cache.Set(cacheName, objectToCache, policy);
        }
    }
}
