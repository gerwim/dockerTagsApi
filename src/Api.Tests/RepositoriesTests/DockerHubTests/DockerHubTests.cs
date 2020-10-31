using System.Threading.Tasks;
using Api.Repositories.DockerHub;
using Api.Repositories.Storage;
using Moq;
using Xunit;

namespace Api.Tests.RepositoriesTests.DockerHubTests
{
    public class DockerHubTests : IClassFixture<TestSetup>
    {
        [Theory]
        [InlineData("gerwim/pfsense-backup", "latest")]
        [InlineData("ubuntu", "latest")]
        public async Task ListTagsReturnsData(string imageName, string searchForRegex)
        {
            // Arrange
            var storageMock = new Mock<IStorage>();
            var storage = storageMock.Object;
            var sut = new DockerHub(storage);
            // Act
            var result = await sut.ListTags(imageName, searchForRegex);
            // Assert
            Assert.True(result.Tags.Count > 0);
        }
    }
}