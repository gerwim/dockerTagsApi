using System;
using System.Collections.Generic;
using System.Linq;
using Api.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Tests.RepositoriesTests
{
    public class RegistryTests : IClassFixture<TestSetup>
    {
        private readonly IServiceProvider _serviceProvider;

        public RegistryTests(TestSetup testSetup)
        {
            _serviceProvider = testSetup.ServiceProvider;
        }
        
        [Fact]
        public void AllRegistriesHaveAnUniqueFriendlyUrl()
        {
            // Arrange
            var registries = _serviceProvider.GetServices<IRegistry>().ToList();
            List<string> friendlyNames = new List<string>();

            // Act
            foreach (var registry in registries)
            {
                friendlyNames.Add(registry.FriendlyUrl);
            }
            
            // Assert
            Assert.True(friendlyNames.Count == friendlyNames.Distinct().Count());
            Assert.DoesNotContain(friendlyNames, String.IsNullOrWhiteSpace);
            Assert.True(friendlyNames.Count == registries.Count);
        }
    }
}