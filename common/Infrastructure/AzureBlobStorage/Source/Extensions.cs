namespace NetSteps.Infrastructure.AzureBlobStorage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The path.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The get all directories in a path.
        /// </summary>
        /// <param name="relativePath">
        /// The relative path.
        /// </param>
        /// <returns>
        /// The list of directory names.
        /// </returns>
        public static List<string> GetDirectories(this string relativePath)
        {
            var parts = new List<string>();

            if (!string.IsNullOrEmpty(relativePath))
            {
                relativePath = relativePath.Replace(@"\", "/");
                var pathParts = relativePath.Split('/');
                parts.AddRange(pathParts.Where(pathPart => !string.IsNullOrEmpty(pathPart)));
            }

            return parts;
        }

        /// <summary>
        /// The get file name.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetFileName(this Uri path)
        {
            var absolutePath = path.AbsolutePath;
            var fileName = Path.GetFileName(absolutePath);
            return fileName;
        }

        /// <summary>
        /// The get relative path.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetRelativePath(this Uri path)
        {
            var fileName = path.GetFileName();
            var relativePath = !string.IsNullOrEmpty(fileName) ? path.AbsolutePath.Replace(fileName, string.Empty) : path.AbsolutePath;
            return relativePath.Trim('/');
        }

        /// <summary>
        /// The remove first directory.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveFirstDirectory(this string path)
        {
            var firstSeparator = path.IndexOf("/", StringComparison.OrdinalIgnoreCase);
            return path.Substring(firstSeparator + 1, path.Length - (firstSeparator + 1));
        }
    }
}
