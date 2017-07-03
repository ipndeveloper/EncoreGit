namespace NetSteps.Infrastructure.Common.Storage 
{
    using System;

    /// <summary>
    /// The StorageProvider interface.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        /// The persist.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="relativePath">
        /// The relative Path.
        /// </param>
        /// <param name="fileName">
        /// The file Name.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        Uri Persist(byte[] file, string relativePath, string fileName);

        /// <summary>
        /// The retrieve.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The byte array of the file.
        /// </returns>
        byte[] Retrieve(Uri path);

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Remove(Uri path);
    }
}
