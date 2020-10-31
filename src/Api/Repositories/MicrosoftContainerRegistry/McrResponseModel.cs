using System.Collections.Generic;

namespace Api.Repositories.MicrosoftContainerRegistry
{
    public class McrResponseModel
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
    }
}