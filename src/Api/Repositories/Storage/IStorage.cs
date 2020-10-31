using System.Threading.Tasks;

namespace Api.Repositories.Storage
{
    public interface IStorage
    {
        Task Write<T>(string key, T value);
        Task<T> Read<T>(string key);
    }
}