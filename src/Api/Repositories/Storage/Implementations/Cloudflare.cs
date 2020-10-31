using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Repositories.Storage.Implementations
{
    public class Cloudflare : BaseStorage
    {
        private readonly IConfiguration _configuration;
        
        public Cloudflare(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override async Task WriteImplementation<T>(string key, T value)
        {
            string json = JsonConvert.SerializeObject(value);
            await WriteToCloudflare(key, json);
        }
        
        protected override async Task<T> ReadImplementation<T>(string key)
        {
            string value = null;
            try
            {
                value = await $"{_configuration["Cloudflare:KVUrl"]}/values/{key}"
                    .WithOAuthBearerToken(_configuration["Cloudflare:ApiToken"])
                    .GetStringAsync();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Message.Contains("404 (Not Found)"))
                {
                    // nothing
                }
                else
                {
                    throw;
                }
            }

            if (value == null) return default;

            T obj = JsonConvert.DeserializeObject<T>(value);
            return obj;
        }
        
        private async Task WriteToCloudflare(string key, string value, int expirationTtl = 86400)
        {
            await
                $"{_configuration["Cloudflare:KVUrl"]}/values/{key}?expiration_ttl={expirationTtl.ToString()}"
                    .WithOAuthBearerToken(_configuration["Cloudflare:ApiToken"])
                    .PutAsync(new StringContent(value));
        }
    }
}