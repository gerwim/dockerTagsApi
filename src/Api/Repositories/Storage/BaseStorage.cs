using System.Threading.Tasks;
using Api.Utils.Extensions;

namespace Api.Repositories.Storage
{
    public abstract class BaseStorage : IStorage
    {
        public Task Write<T>(string key, T value)
        {
            // Convert the key
            string encodedKey = $"{typeof(T)}-{key}".ComputeSha256Hash();
            
            return this.WriteImplementation<T>(encodedKey, value);
        }

        public Task<T> Read<T>(string key)
        {
            // Convert the key
            string encodedKey = $"{typeof(T)}-{key}".ComputeSha256Hash();

            return this.ReadImplementation<T>(encodedKey);
        }

        protected abstract Task<T> ReadImplementation<T>(string key);
        protected abstract Task WriteImplementation<T>(string key, T value);
    }
}