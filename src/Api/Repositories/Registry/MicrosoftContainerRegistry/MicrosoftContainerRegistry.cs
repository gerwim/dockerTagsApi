using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api.Models;
using Api.Repositories.Storage;
using Flurl.Http;

namespace Api.Repositories.Registry.MicrosoftContainerRegistry
{
    public class MicrosoftContainerRegistry : IRegistry
    {
        public string FriendlyUrl => "mcr.microsoft.com";
        private const string HttpEndpoint = "https://mcr.microsoft.com/v2";
        
        private readonly IStorage _storage;

        public MicrosoftContainerRegistry(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<TagsReponseModel> ListTags(string imageName, string searchForRegex)
        {
            var mcr = await GetTags(imageName);
            var regex = new Regex(searchForRegex);
            var result = new TagsReponseModel
            {
                Name = imageName,
                Tags = mcr.Tags.Where(x => regex.IsMatch(x)).ToList()
            };

            return result;
        }

        private async Task<McrResponseModel> GetTags(string imageName)
        {
            var tags = await _storage.Read<McrResponseModel>(imageName);
            if (tags != null)
                return tags;
          
            var response = await $"{HttpEndpoint}/{imageName}/tags/list".GetJsonAsync<McrResponseModel>();
            await _storage.Write(imageName, response); // cache the value
            return response;
        }
        
    }
}