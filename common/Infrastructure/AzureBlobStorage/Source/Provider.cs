namespace NetSteps.Infrastructure.AzureBlobStorage
{
    using System;
    using System.IO;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using NetSteps.Infrastructure.Common.Storage;

    /// <summary>
    /// The provider.
    /// </summary>
    public class Provider : IStorageProvider
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly CloudBlobContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Provider"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="containerName">
        /// The container name.
        /// </param>
        public Provider(string connectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            var storageClient = storageAccount.CreateCloudBlobClient();
            this.container = storageClient.GetContainerReference(containerName.ToLowerInvariant());
            this.container.CreateIfNotExists();
        }

        /// <summary>
        /// The persist.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="relativePath">
        /// The relative path.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public Uri Persist(byte[] file, string relativePath, string fileName)
        {
            var directory = this.GetDirectoryFromRelativePath(relativePath);
            var blob = directory != null ? directory.GetBlockBlobReference(fileName.ToLowerInvariant()) : this.container.GetBlockBlobReference(fileName.ToLowerInvariant());

            using (var stream = new MemoryStream(file))
            {
                blob.UploadFromStream(stream);
            }

            return blob.Uri;
        }

        /// <summary>
        /// The retrieve.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The byte array of the file.
        /// </returns>
        public byte[] Retrieve(Uri path)
        {
            var blob = this.GetBlobReference(path);
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Remove(Uri path)
        {
            var blob = this.GetBlobReference(path);
            return blob.DeleteIfExists();
        }

        /// <summary>
        /// The get blob reference.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="CloudBlockBlob"/>.
        /// </returns>
        private CloudBlockBlob GetBlobReference(Uri path)
        {
            var relativePath = path.GetRelativePath().RemoveFirstDirectory();
            var directory = this.GetDirectoryFromRelativePath(relativePath);
            var fileName = path.GetFileName();
            CloudBlockBlob blob = directory != null ? directory.GetBlockBlobReference(fileName) : this.container.GetBlockBlobReference(fileName);
            return blob;
        }

        /// <summary>
        /// The get directory from relative path.
        /// </summary>
        /// <param name="relativePath">
        /// The relative path.
        /// </param>
        /// <returns>
        /// The <see cref="CloudBlobDirectory"/>.
        /// </returns>
        private CloudBlobDirectory GetDirectoryFromRelativePath(string relativePath)
        {
            CloudBlobDirectory directory = null;
            var parts = relativePath.GetDirectories();
            foreach (var part in parts)
            {
                directory = this.container.GetDirectoryReference(part);
            }

            return directory;
        }
    }
}
