using System.Text.RegularExpressions;

namespace Api.Repositories.DockerHub
{
    public static class ImageNameParser
    {
        public static string ParseImageName(string imageName)
        {
            switch (imageName)
            {
                case var customRepository when new Regex(@"(.*)/(.*)").IsMatch(customRepository):
                    return $"{imageName}";
                case var officialRepository when new Regex(@"(.*)").IsMatch(officialRepository):
                    return $"library/{imageName}";
                default:
                    return $"{imageName}";
            }   
        }
    }
}