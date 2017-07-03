using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using NetSteps.Objects.Business;
using NetSteps.Web.Base;

namespace NetSteps.Web.Handlers
{
    public class DocumentHandler : BaseHttpHandler
    {
        #region Constants
        private const string FILE_NAME = "FileName";
        private const string ARCHIVEID = "Arc";
        #endregion

        #region Members
        private ImageFormat _formatType = ImageFormat.Gif;
        private string ErrorMsg = string.Empty;
        #endregion

        #region Methods
        protected byte[] GetFileBytes(string filePath)
        {
            try
            {
                filePath = WebContext.WebConfig.ImagesAbsolutePath + filePath.Replace("/", "\\");

                FileInfo file = new FileInfo(filePath);

                // Create the byte array which contains the file. - JHE
                FileStream fs = file.OpenRead();
                byte[] fileAsBytes = new byte[fs.Length];
                fs.Read(fileAsBytes, 0, (int)fs.Length);
                fs.Close();

                return fileAsBytes;
            }
            catch (System.Exception ex)
            {
                ErrorMsg = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                ErrorMsg += "<LI>" + ex.GetBaseException().ToString() + "<hr>\r\n\r\n";
                ErrorMsg += "<LI><a href='" + filePath + "'>" + filePath + "</a><br /><br />\r\n\r\n";
                return null;
            }
        }

        protected void DisplayFile(byte[] fileByteArray, string fileName)
        {
            string contentType = GetContentTypeFromFileName(fileName);
            if (contentType == "video/x-ms-wmv" || contentType == "audio/wav") // If WMV video - JHE
            {
                string filePath = WebContext.WebConfig.ImagesAbsolutePath + fileName.Replace("/", "\\");
                DisplayMediaFile(filePath);
            }
            else if (contentType == "video/H264")
            {
                _response.ContentType = contentType;

                string filePath = WebContext.WebConfig.ImagesAbsolutePath + fileName.Replace("/", "\\");
                byte[] buffer = new byte[4096];
                using (FileStream stream = File.OpenRead(filePath))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer, 0, count);
                    } while (count != 0);

                    memoryStream.WriteTo(_response.OutputStream);
                }
            }
            else if (fileByteArray != null)
            {
                _response.Clear();
                _response.ClearContent();
                _response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                _response.ContentType = contentType;
                _response.OutputStream.Write(fileByteArray, 0, fileByteArray.Length);
                _response.End();
            }
            else
            {
                _response.Write("<html><body>");
                _response.Write("We are sorry the requested file <b>" + fileName + "</b> could not retrieved.");
                _response.Write("<hr />" + ErrorMsg);
                _response.Write("</body></html>");
            }
        }

        /// <summary>
        /// Taken from http://www.codeverge.net/item.aspx?item=394989 - JHE
        /// </summary>
        /// <param name="fileByteArray"></param>
        /// <param name="filepath"></param>
        protected void DisplayMediaFile(string filepath)
        {
            System.IO.Stream stream = null;
            // Buffer to read 1000K bytes in chunk:
            byte[] buffer = new Byte[1000000];
            // Length of the file:
            int length;
            // Total bytes to read:
            long dataToRead;
            // Identify the file to download including its path.
            //string filepath = "c:\\spiderman.wmv";
            // Identify the file name.

            string filename = System.IO.Path.GetFileName(filepath);
            try
            {
                // Open the file.
                stream = new System.IO.FileStream(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                // Total bytes to read:
                dataToRead = stream.Length;

                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);

                // Read the bytes.
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = stream.Read(buffer, 0, 1000000);

                        // Write the data to the current output stream.
                        Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        Response.Flush();

                        buffer = new Byte[1000000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        // Prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    // Close the file.
                    stream.Close();
                }
            }
        }
        #endregion

        #region BaseHttpHandler Overrides
        public override void SetResponseCachePolicy(HttpCachePolicy cache)
        {
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.AddDays(14));
            cache.SetValidUntilExpires(true);
        }

        /// <summary>
        /// Main interface for reacting to the Thumbnailer request.
        /// </summary>
        /// <param name="context"></param>
        protected override void HandleRequest(HttpContext context)
        {
            string fileName = GetQueryStringVar<string>(FILE_NAME, string.Empty);
            int archiveId = GetQueryStringVar<int>(ARCHIVEID);

            if (archiveId != 0)
            {
                // Increment count - JHE
                Archive archive = Archive.Find(archiveId);
                archive.Downloads = archive.Downloads + 1;
                archive.Save();
            }

            if (fileName == string.Empty)
                WriteOutSimpleTextResponse("The name of file to get was not provided on the QueryString!", context);

            DisplayFile(GetFileBytes(fileName), fileName);
        }
        #endregion
    }
}
