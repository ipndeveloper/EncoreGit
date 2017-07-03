using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetSteps.Common.Extensions;

namespace NetSteps.Common
{
    public class IO
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

        /// <summary>
        /// Remember to add the root path to these images to the return value. - JHE
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetThumbnailFromFileType(string fileName)
        {
            string ext = IO.GetFileExtention(fileName);
            string returnThumbnail = string.Empty;

            switch (ext)
            {
                case ".pdf":
                    returnThumbnail = "acrobatICO.gif";
                    break;
                case ".mp3":
                case ".wav":
                    returnThumbnail = "audioICO.gif";
                    break;
                case ".swf":
                case ".flv":
                    returnThumbnail = "flashICO.gif";
                    break;
                case ".mov":
                    returnThumbnail = "movieICO.gif";
                    break;
                case ".avi":
                case ".mpg":
                case ".mpeg":
                    returnThumbnail = "videoICO.gif";
                    break;
                case ".doc":
                case ".docx":
                    returnThumbnail = "wordICO.gif";
                    break;
                case ".xl":
                case ".xls":
                case ".xlsx":
                case ".xml":
                    returnThumbnail = "excelICO.gif";
                    break;
                default:
                    returnThumbnail = "genericdocICO.gif";
                    break;
            }

            return returnThumbnail;
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

        /// <summary>
        /// Determines if a file exists
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>true if it exists, false otherwise</returns>
        public static bool FileExists(string fileName)
        {
            try
            {
                return File.Exists(fileName);
            }
            catch (Exception a)
            {
                throw a;
            }
        }

        /// <summary>
        /// Gets a files' contents
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>a string containing the file's contents</returns>
        public static string GetFileContents(string fileName)
        {
            StreamReader streamReader = null;
            try
            {
                if (!FileExists(fileName))
                    return string.Empty;
                bool opened = false;
                while (!opened)
                {
                    try
                    {
                        streamReader = File.OpenText(fileName);
                        opened = true;
                    }
                    catch (System.IO.IOException e)
                    {
                        throw e;
                    }
                }
                string contents = streamReader.ReadToEnd();
                streamReader.Close();
                return contents;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Close();
            }
        }
        #endregion



        /// <summary>
        /// Serialize the object to a file
        /// </summary>
        /// <param name="obj">Object to be serialized.Ensure that is Serializable !</param>
        /// <param name="filePath">File( with the entire file path) where the object will be serialized to</param>
        /// <returns>True on successful serialization.</returns>
        public static bool FileSerialize(Object obj, string filePath)
        {
            FileStream fileStream = null;

            try
            {
                //if (FileExists(filePath))
                //{
                //    FileInfo file = new FileInfo(filePath);
                //    file.Delete();
                //}

                fileStream = new FileStream(filePath, FileMode.Create);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, obj);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }

            return true;
        }

        /// <summary>
        /// Deserializes a binary formatted object.
        /// </summary>
        /// <param name="filePath">Full path of the file</param>
        /// <returns>The deserialized object</returns>
        public static T FileDeSerialize<T>(string filePath)
        {
            FileStream fileStream = null;
            Object obj;
            try
            {
                if (File.Exists(filePath) == false)
                    throw new FileNotFoundException("The file was not found.", filePath);

                fileStream = new FileStream(filePath, FileMode.Open);
                BinaryFormatter b = new BinaryFormatter();
                obj = b.Deserialize(fileStream);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }

            return (T)obj;
        }

        public static string[] GetFileTypeExtenstions(Constants.FileType fileType)
        {
            if (fileType == Constants.FileType.Image)
            {
                return new string[] { ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".tif" };
            }
            else if (fileType == Constants.FileType.Audio)
            {
                return new string[] { ".mp3", ".wav", ".wma" };
            }
            else if (fileType == Constants.FileType.PDF)
            {
                return new string[] { ".pdf" };
            }
            else if (fileType == Constants.FileType.Video)
            {
                return new string[] { ".mov", ".wmv", ".avi", ".divx", ".mp4", ".mpeg" };
            }
            else if (fileType == Constants.FileType.Flash)
            {
                return new string[] { ".flv", ".swf" };
            }
            else if (fileType == Constants.FileType.Word)
            {
                return new string[] { ".doc", ".docx", ".rtf", ".txt", ".wpd" };
            }
            else if (fileType == Constants.FileType.Excel)
            {
                return new string[] { ".csv", ".xls", ".xlsx", ".txt", ".wpd" };
            }
            else if (fileType == Constants.FileType.Powerpoint)
            {
                return new string[] { ".ppt", ".pptx" };
            }
            else if (fileType == Constants.FileType.Archive)
            {
                return new string[] { ".zip", ".7z", ".rar", ".zipx", ".tar" };
            }

            return new string[] { };
        }
    }
}
