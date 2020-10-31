using System.Collections.Generic;

namespace Api.Repositories.DockerHub
{
    public class DockerHubResponseModel
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
    }
}