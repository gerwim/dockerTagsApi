using System.Threading.Tasks;
using Api.Repositories.Registry.MicrosoftContainerRegistry;
using Api.Repositories.Storage;
using Moq;
using Xunit;

namespace Api.Tests.RepositoriesTests.MicrosoftContainerRegistryTests
{
    public class MicrosoftContainerRegistryTests : IClassFixture<TestSetup>
    {
        [Fact]
        public async Task ListTagsReturnsData()
        {
            // Arrange
            var storageMock = new Mock<IStorage>();

            var storage = storageMock.Object;
            var sut = new MicrosoftContainerRegistry(storage);
            // Act
            var result = await sut.ListTags("dotnet/framework/aspnet", @"^4\.8-windowsservercore-(\d{4}|ltsc\d{4})");
            // Assert
            Assert.True(result.Tags.Count > 0);
        }
    }
}