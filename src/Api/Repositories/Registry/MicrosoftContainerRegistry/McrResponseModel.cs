using System.Collections.Generic;

namespace Api.Repositories.Registry.MicrosoftContainerRegistry
{
    public class McrResponseModel
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
    }
}