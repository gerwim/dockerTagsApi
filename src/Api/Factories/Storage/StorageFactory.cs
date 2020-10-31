using System;
using Api.Repositories.Storage;
using Api.Repositories.Storage.Implementations;
using Microsoft.Extensions.Configuration;

namespace Api.Factories.Storage
{
    public static class StorageFactory
    {
        public static IStorage CreateStorage(IConfiguration configuration)
        {
            if (!String.IsNullOrWhiteSpace(configuration["Cloudflare:KVUrl"]))
            {
                return new Cloudflare(configuration);
            }

            // Default to an in memory cache
            return new InMemoryCache();
        }
    }
}