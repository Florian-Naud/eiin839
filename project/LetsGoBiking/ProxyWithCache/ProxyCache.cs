using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ProxyWithCache
{
    class ProxyCache<T>
    {
        private ObjectCache cache;
        private DateTimeOffset dt_default;
        private String apikey;

        public ProxyCache()
        {
            cache = MemoryCache.Default;
            dt_default = ObjectCache.InfiniteAbsoluteExpiration;
            apikey = "aadb7f161ebe9eb9358509283772daab3be2898b";
            
        }

        public async Task<String> initialise()
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + apikey);
        }

        public List<T> GetAll()
        {
            List<T> all = new List<T>();
            foreach (var item in cache)
            {
                all.Add((T)item.Value);
            }
            return all;
        }

        public T Get(String CacheItem) // cache is key of the entry cache
        {
            return this.Get(CacheItem, this.dt_default);
        }

        public T Get(string CacheItem, double dt_seconds)
        {
            return this.Get(CacheItem, DateTimeOffset.Now.AddSeconds(dt_seconds));
        }

        public T Get(string CacheItem, DateTimeOffset dt)
        {
            var item = cache.Get(CacheItem);

            if (item == null)
            {
                this.Set(CacheItem, default(T), dt);
            }

            return (T)cache.Get(CacheItem);
        }

        public void Set(String CacheItem, T obj) // cache is key of the entry cache
        {
            this.Set(CacheItem, obj, this.dt_default);
        }

        public void Set(string CacheItem, T obj, double dt_seconds)
        {
            this.Set(CacheItem, obj, DateTimeOffset.Now.AddSeconds(dt_seconds));
        }

        public void Set(string CacheItem, T obj, DateTimeOffset dt)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = dt,
            };
            cache.Set(CacheItem, obj, cacheItemPolicy);
        }
    }
}
