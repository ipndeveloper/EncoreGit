using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;

namespace NetSteps.Web.ContentFileHandler
{
    internal static class FileSync
    {
        internal static void SavePostedFile(HttpPostedFile file)
        {
            var path = ConfigurationManager.AppSettings["FileUploadAbsolutePath"];
            if (!path.EndsWith("\\"))
                path += "\\";
            file.SaveAs(path + file.FileName);
        }

        internal static bool SyncFile(string localFilePath, string relativePath)
        {
            var sharedFilePath = getSharedPath(relativePath);
            var localDirectory = localFilePath.Substring(0, localFilePath.LastIndexOf('\\') + 1);

            if (!Directory.Exists(localDirectory))
            {
                Directory.CreateDirectory(localDirectory);
            }

            if (File.Exists(localFilePath))
            {
                if (!File.Exists(sharedFilePath))
                {
                    File.Copy(localFilePath, sharedFilePath, true);
                }
                else
                {
                    var fileVersionCheck = checkFileVersions(localFilePath, sharedFilePath);
                    switch (fileVersionCheck)
                    {
                        case FileVersionResult.Equal:
                            break;
                        case FileVersionResult.LocalFileNewer:
                            File.Copy(localFilePath, sharedFilePath, true);
                            break;
                        case FileVersionResult.SharedFileNewer:
                            File.Copy(sharedFilePath, localFilePath, true);
                            break;
                        default:
                            throw new Exception("Unhandled internal file version check result.");
                    }
                }
            }
            else
            {
                // local file doesn't exist, check shared path
                if (!File.Exists(sharedFilePath))
                {
                    return false;
                }
                else
                {
                    File.Copy(sharedFilePath, localFilePath, true);
                }
            }
            return true;
        }

        private static string getSharedPath(string relativeFilePath)
        {
            return ConfigurationManager.AppSettings["FileUploadAbsolutePath"] + relativeFilePath.Replace("/fileuploads", "").Replace("/", "\\");
        }

        private static FileVersionResult checkFileVersions(string localFilePath, string sharedFilePath)
        {
            var localVersion = File.GetLastWriteTimeUtc(localFilePath);
            var sharedVersion = File.GetLastWriteTimeUtc(sharedFilePath);

            if (localVersion == sharedVersion)
                return FileVersionResult.Equal;
            else if (localVersion > sharedVersion)
                return FileVersionResult.LocalFileNewer;
            else
                return FileVersionResult.SharedFileNewer;
        }

        private enum FileVersionResult
        {
            Equal,
            SharedFileNewer,
            LocalFileNewer
        }

    }


}
