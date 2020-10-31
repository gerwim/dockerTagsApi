using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api.Models;
using Api.Repositories.Storage;
using Flurl;
using Flurl.Http;

namespace Api.Repositories.DockerHub
{
    public class DockerHub : IRegistry
    {
        public string FriendlyUrl => "hub.docker.com";
        private const string HttpEndpoint = "https://index.docker.io/v2";
        
        private readonly IStorage _storage;

        public DockerHub(IStorage storage)
        {
            _storage = storage;
        }
        
        public async Task<TagsReponseModel> ListTags(string imageName, string searchForRegex)
        {
            var regex = new Regex(searchForRegex);
            string convertedImageName = ImageNameParser.ParseImageName(imageName);
            var dockerHubTags = await GetTagsFromDockerHub(convertedImageName);

            var result = new TagsReponseModel
            {
                Name = imageName,
                Tags = dockerHubTags.Tags.Where(x => regex.IsMatch(x)).ToList()
            };

            return result;
        }
        
        private async Task<DockerHubResponseModel> GetTagsFromDockerHub(string imageName)
        {
            var dockerHubResponse = await _storage.Read<DockerHubResponseModel>(imageName);
            if (dockerHubResponse != null)
                return dockerHubResponse;

            string bearerToken = await GetBearerToken(imageName);
            
            var response = await $"{HttpEndpoint}/{imageName}/tags/list"
                    .WithOAuthBearerToken(bearerToken)
                    .GetJsonAsync<DockerHubResponseModel>();

            await _storage.Write(imageName, response); // cache the value
            return response;
        }

        private async Task<string> GetBearerToken(string imageName)
        {
            string authHeader = null;
            // First do an unauthenticated request to get the realm, service and scope
            try
            {
                await $"{HttpEndpoint}/{imageName}/tags/list".GetJsonAsync();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus != HttpStatusCode.Unauthorized) // we expect a 401 here
                    throw;

                authHeader = ex.Call.Response.Headers.WwwAuthenticate.FirstOrDefault()?.ToString();
            }
            
            if (authHeader == null)
                return String.Empty;

            string realm = Extract(authHeader, "realm");
            string service = Extract(authHeader, "service");
            string scope = Extract(authHeader, "scope");

            // Retrieve a token from the API
            dynamic tokenObject = await realm
                .SetQueryParam("service", service)
                .SetQueryParam("scope", scope)
                .GetJsonAsync();
            
            return tokenObject.token;
        }

        // Input will be in the format:
        // Bearer realm="https://auth.docker.io/token",service="registry.docker.io",scope="repository:gerwim/some-image:pull"
        private string Extract(string authHeader, string name)
        {
            var param = authHeader.Split($"{name}=")[1].Remove(0, 1);
            var quoteCharPosition = param.IndexOf('"');

            return param.Substring(0, quoteCharPosition);
        }
    }
}