using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Repositories.Storage.Implementations
{
    public class InMemoryCache : BaseStorage
    {
        private ConcurrentDictionary<string, (DateTime expireAtUtc, string data)> LocalCache { get; }

        public InMemoryCache()
        {
            this.LocalCache = new ConcurrentDictionary<string, (DateTime expireAtUtc, string data)>();
        }
        protected override Task WriteImplementation<T>(string key, T value)
        {
            LocalCache[key] = (DateTime.UtcNow.AddHours(24), JsonConvert.SerializeObject(value));
            return Task.CompletedTask;
        }

        protected override Task<T> ReadImplementation<T>(string key)
        {
            if (!LocalCache.TryGetValue(key, out var value))
                return Task.FromResult<T>(default);

            if (DateTime.UtcNow <= value.expireAtUtc)
                return Task.FromResult(JsonConvert.DeserializeObject<T>(value.data));
            
            LocalCache.TryRemove(key, out _);
            return Task.FromResult<T>(default);
        }
    }
}