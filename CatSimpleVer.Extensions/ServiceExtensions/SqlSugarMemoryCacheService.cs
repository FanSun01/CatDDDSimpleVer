using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;

namespace CatSimpleVer.Extensions
{
    public class SqlSugarMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public SqlSugarMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add<V>(string key, V value)
        {
            _memoryCache.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            _memoryCache.Set(key, value, DateTimeOffset.Now.AddSeconds(cacheDurationInSeconds));
        }

        public bool ContainsKey<V>(string key)
        {
            return _memoryCache.TryGetValue(key, out bool result);
        }

        public V Get<V>(string key)
        {
            return _memoryCache.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out V value))
            {
                value = create();
                _memoryCache.Set(cacheKey, value, DateTimeOffset.Now.AddSeconds(cacheDurationInSeconds));
            }
            return value;
        }

        public void Remove<V>(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
