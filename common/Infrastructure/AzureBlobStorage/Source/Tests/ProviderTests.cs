namespace AzureBlobStorage.Tests
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NetSteps.Infrastructure.AzureBlobStorage;

    /// <summary>
    /// The provider tests.
    /// </summary>
    [TestClass]
    public class ProviderTests
    {
        /// <summary>
        /// The expect provider to place blob into storage.
        /// </summary>
        [TestMethod]
        public void ExpectProviderToPlaceBlobIntoStorage()
        {
            // Arrange
            const string ConnectionString = @"DefaultEndpointsProtocol=https;AccountName=nstesting;AccountKey=jVDM3scJgK0DPt7cvtUn/RtdV8VRlmgUGkIcTn80zzOS9+BdYdRrdcSRQW+MB8zG51zaQgp/fCE23wFoK5kwKg==";
            var provider = new Provider(ConnectionString, "integrationtests");
            var executingProjectRoot = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.Parent.Parent.FullName;
            var fileContents = File.ReadAllBytes(Directory.GetFiles(Path.Combine(executingProjectRoot, "TestFiles")).First(f => f.EndsWith("test.jpg")));

            // Act
            var uri = provider.Persist(fileContents, "providerTests", "test.jpg");

            // Assert
            Assert.AreEqual("https://nstesting.blob.core.windows.net/integrationtests/providerTests/test.jpg", uri.ToString());

            // Act Again
            var retrievedBytes = provider.Retrieve(uri);

            // Assert Again
            Assert.IsTrue(retrievedBytes.Length > 0);
            Assert.AreEqual(fileContents.Length, retrievedBytes.Length);

            // Act Again
            var removed = provider.Remove(uri);

            // Assert Again
            Assert.IsTrue(removed);
        }
    }
}
