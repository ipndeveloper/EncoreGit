using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight
{
    public static class IO
    {
        #region Image Methods
        public static bool IsValidImage(string fileName)
        {
            string ext = IO.GetFileExtention(fileName);

            bool isValid = false;
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    isValid = true;
                    break;
                case ".gif":
                    isValid = true;
                    break;
                case ".png":
                    isValid = true;
                    break;
                case ".bmp":
                    isValid = true;
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        public static List<string> RemoveNonImageFiles(List<string> list)
        {
            List<string> returnList = new List<string>();

            foreach (string filePath in list)
                if (IO.IsValidImage(filePath))
                    returnList.Add(filePath);

            return returnList;
        }
        #endregion

        #region File Methods
        public static string GetFileExtention(string fileName)
        {
            string ext = string.Empty;
            try
            {
                ext = Path.GetExtension(fileName).ToLower();
            }
            catch
            {
                // Invalid Path: just get extention manually - JHE
                string extention = string.Empty;
                if (fileName.Contains("."))
                    ext = fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf(".")).ToLower();
            }
            return ext;
        }

        public static string GetFileName(string fileName)
        {
            int startPoint = fileName.LastIndexOf('/') + 1;
            string returnValue = fileName.Substring(startPoint);
            return returnValue;
        }

        public static string GetFileNameWithoutExtention(string fileName)
        {
            int startPoint = fileName.LastIndexOf('/') + 1;
            int endPoint = fileName.LastIndexOf('.');
            string returnValue = fileName.Substring(startPoint, fileName.Length - startPoint - (fileName.Length - endPoint)).PascalToSpaced();
            return returnValue;
        }
        #endregion

        #region File size Methods
        /// <summary>
        /// Converts a numeric value into a string that represents the number
        /// expressed as a size value in bytes, kilobytes, megabytes, or
        /// gigabytes, depending on the size.
        /// </summary>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        public static string FormatFileSize(int fileSize)
        {
            return FormatFileSize((long)fileSize);
        }

        /// <summary>
        /// Converts a numeric value into a string that represents the number
        /// expressed as a size value in bytes, kilobytes, megabytes, or
        /// gigabytes, depending on the size.
        /// </summary>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        public static string FormatFileSize(long fileSize)
        {
            const long fileSize1KB = 1024;
            const long fileSize100KB = 102400;
            const long fileSize1MB = 1048576;
            const long fileSize1GB = 1073741824;
            const long fileSize1TB = 1099511627776;

            string returnMessage = string.Empty;
            if (fileSize < fileSize1KB)
            {
                returnMessage = string.Format(
                    Thread.CurrentThread.CurrentCulture,
                    @"{0} bytes",
                    fileSize);
            }
            else if (fileSize < fileSize100KB)
            {
                returnMessage = string.Format(
                    Thread.CurrentThread.CurrentCulture,
                    @"{0:F1} KB",
                    (double)fileSize / (double)fileSize1KB);
            }
            else if (fileSize < fileSize1MB)
            {
                returnMessage = string.Format(
                    Thread.CurrentThread.CurrentCulture,
                    @"{0} KB",
                    fileSize / fileSize1KB);
            }
            else if (fileSize < fileSize1GB)
            {
                returnMessage = string.Format(
                    Thread.CurrentThread.CurrentCulture,
                    @"{0:F1} MB",
                    (double)fileSize / (double)fileSize1MB);
            }
            else if (fileSize < fileSize1TB)
            {
                returnMessage = string.Format(
                    Thread.CurrentThread.CurrentCulture,
                    @"{0:F2} GB",
                    (double)fileSize / (double)fileSize1GB);
            }
            else
            {
                returnMessage = string.Format(
                    Thread.CurrentThread.CurrentCulture,
                    @"{0:F2} TB",
                    (double)fileSize / (double)fileSize1TB);
            }

            if (returnMessage.Contains(".0 "))
                returnMessage = returnMessage.Replace(".0 ", string.Empty);
            return returnMessage;
        }
        #endregion
    }

    public static class FileSize
    {
        public static long FromMegaBytes(int fileSize)
        {
            return Convert.ToInt64(1024f * 1024f * fileSize);
        }
    }
}
