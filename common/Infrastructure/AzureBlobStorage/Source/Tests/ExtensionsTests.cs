namespace AzureBlobStorage.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NetSteps.Infrastructure.AzureBlobStorage;

    /// <summary>
    /// The path tests.
    /// </summary>
    [TestClass]
    public class ExtensionsTests
    {
        /// <summary>
        /// The get file name returns filename from url.
        /// </summary>
        [TestMethod]
        public void GetFileNameReturnsFilenameFromUrl()
        {
            // Arrange
            var uri = new Uri("http://google.com/test/test.jpg");

            // Act
            var fileName = uri.GetFileName();

            // Assert
            Assert.AreEqual("test.jpg", fileName);
        }

        /// <summary>
        /// The get directories returns two directories.
        /// </summary>
        [TestMethod]
        public void GetDirectoriesReturnsTwoDirectories()
        {
            // Arrange
            const string RelativePath = "/test/test2/";

            // Act
            var directories = RelativePath.GetDirectories();

            // Assert
            Assert.IsTrue(directories.Count == 2);
            Assert.AreEqual("test", directories[0]);
            Assert.AreEqual("test2", directories[1]);
        }

        /// <summary>
        /// The get relative path returns path without file name.
        /// </summary>
        [TestMethod]
        public void GetRelativePathReturnsPathWithoutFileName()
        {
            // Arrange
            var uri = new Uri("http://google.com/test/test2/test.jpg");

            // Act
            var relativepath = uri.GetRelativePath();

            // Assert
            Assert.AreEqual("test/test2", relativepath);
        }

        /// <summary>
        /// The remove first directory removes directory.
        /// </summary>
        [TestMethod]
        public void RemoveFirstDirectoryRemovesDirectory()
        {
            // Arrange
            const string RelativePath = "test/test2";

            // Act
            var truncatedPath = RelativePath.RemoveFirstDirectory();

            // Assert
            Assert.AreEqual("test2", truncatedPath);
        }
    }
}
