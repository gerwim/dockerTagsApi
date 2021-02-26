using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Repositories.Storage.Implementations
{
    public class InMemoryCache : BaseStorage
    {
        private readonly int _expirationTtl;
        private ConcurrentDictionary<string, (DateTime expireAtUtc, string data)> LocalCache { get; }

        
        public InMemoryCache(IConfiguration configuration)
        {
            this.LocalCache = new ConcurrentDictionary<string, (DateTime expireAtUtc, string data)>();
            try
            {
                _expirationTtl = String.IsNullOrWhiteSpace(configuration["Cache:ExpirationTtl"])
                    ? 86400
                    : Convert.ToInt32(configuration["Cache:ExpirationTtl"]);
            }
            catch
            {
                _expirationTtl = 86400;
            }
        }
        protected override Task WriteImplementation<T>(string key, T value)
        {
            LocalCache[key] = (DateTime.UtcNow.AddSeconds(_expirationTtl), JsonConvert.SerializeObject(value));
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