using Api.Repositories.DockerHub;
using Xunit;

namespace Api.Tests.RepositoriesTests.DockerHubTests
{
    public class ImageNameParserTests
    {
        
        [Theory]
        [InlineData("gerwim/pfsense-backup", "gerwim/pfsense-backup")]
        [InlineData("ubuntu", "library/ubuntu")]
        public void ParseImageName_Correct(string imageName, string expectedValue)
        {
            // Arrange
            
            // Act
            var result = ImageNameParser.ParseImageName(imageName);
            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}