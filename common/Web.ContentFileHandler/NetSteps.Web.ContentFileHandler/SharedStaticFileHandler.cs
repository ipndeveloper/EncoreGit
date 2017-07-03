using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Configuration;
using System.Net;

namespace NetSteps.Web.ContentFileHandler
{
    public class SharedStaticFileHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request.RequestType)
            {
                case "POST":
                    PostHandler(context);
                    break;
                case "GET":
                    GetHandler(context);
                    break;
            }
        }

        private void PostHandler(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                foreach (HttpPostedFile file in context.Request.Files)
                {
                    FileSync.SavePostedFile(file);
                }
                context.Response.Status = "File Uploaded";
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                context.Response.End();
            }
            else
            {
                context.Response.Status = "No File Found";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.End();
            }
        }

        private void GetHandler(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;
            
            var relativePath = request.Url.AbsolutePath.ToLower();
            var localFilePath = context.Server.MapPath(relativePath);

            if (FileSync.SyncFile(localFilePath, relativePath))
            {
                response.ContentType = getContentType(relativePath);
                response.WriteFile(localFilePath);
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Flush();
            }
            else
            {
                response.Status = "404 Not Found";
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            response.End();
        }


        
        private string getContentType(string relativePath)
        {
            var extension = relativePath.Remove(0, relativePath.LastIndexOf('.') + 1);
            switch (extension)
            {
                case "png":
                    return "image/png";
                case "jpg":
                case "jpeg":
                case "jpe":
                    return "image/jpeg";
                case "bmp":
                    return "image/bmp";
                case "gif":
                    return "image/gif";
                case "css":
                    return "text/css";
                case "pdf":
                    return "application/pdf";
                case "avi":
                    return "video/x-msvideo";
                case "qt":
                case "mov":
                    return "video/quicktime";
                case "mpeg":
                case "mpe":
                case "mpg":
                    return "video/mpeg";
                default:
                    return "text/html";
            }
        }
    }
}
