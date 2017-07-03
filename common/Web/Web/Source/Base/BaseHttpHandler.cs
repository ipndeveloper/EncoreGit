using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A bass class for HttpHandlers 
    /// Created: 04-18-2009
    /// </summary>
    public abstract class BaseHttpHandler : IHttpHandler
    {
        #region Members
        protected string _mimeType = "text/html";
        protected string _errorMessage = string.Empty;
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether this handler requires users to be authenticated.
        /// </summary>
        /// <value>
        ///    <c>true</c> if authentication is required
        ///    otherwise, <c>false</c>.
        /// </value>
        public virtual bool RequiresAuthentication
        {
            get { return false; }
        }
        public string Cache { get; set; }
        public string UrlHash { get; set; }

        /// <summary>
        /// Gets the MIME Type.
        /// </summary>
        public virtual string ContentMimeType
        {
            get { return _mimeType; }
        }
        public bool IsReusable
        {
            get { return true; }
        }

        public bool IsImageMimeType()
        {
            if (_mimeType == "image/jpeg" || _mimeType == "image/gif" || _mimeType == "image/png" || _mimeType == "image/bmp")
                return true;
            else
                return false;
        }

        public bool ErrorOccured
        {
            get { return !string.IsNullOrEmpty(_errorMessage); }
        }

        protected HttpRequest _request;
        public HttpRequest Request
        {
            get { return _request; }
            set { _request = value; }
        }

        protected HttpResponse _response;
        public HttpResponse Response
        {
            get { return _response; }
            set { _response = value; }
        }
        #endregion

        #region Constructors
        public BaseHttpHandler()
        {
        }
        #endregion Constructors

        #region Methods
        public void EnsureIsImageMimeType(HttpContext context)
        {
            if (!IsImageMimeType())
                context.Response.ContentType = "image/png";
        }

        /// <summary>
        /// Process the incoming HTTP request.
        /// </summary>
        /// <param name="context">Context.</param>
        public void ProcessRequest(HttpContext context)
        {
            _request = context.Request;
            _response = context.Response;

            Cache = context.Request.Url.AbsoluteUri;
            UrlHash = Cache.GetMd5Sum();

            if (!this.ValidateParameters(context))
            {
                this.RespondInternalError(context);
                return;
            }

            if (this.RequiresAuthentication && !context.User.Identity.IsAuthenticated)
            {
                this.RespondForbidden(context);
                return;
            }

            this.SetResponseCachePolicy(context.Response.Cache);
            context.Response.AppendHeader("Etag", UrlHash);
            context.Response.ContentType = this.ContentMimeType;
            this.HandleRequest(context);
        }

        /// <summary>
        /// Handles the request.  This is where you put your business logic.
        /// </summary>
        /// <remarks>
        /// <p>This method should result in a call to one (or more) of the following methods:</p>
        /// <p><code>context.Response.BinaryWrite();</code></p>
        /// <p><code>context.Response.Write();</code></p>
        /// <p><code>context.Response.WriteFile();</code></p>
        /// <p>
        /// <code>
        /// someStream.Save(context.Response.OutputStream);
        /// </code>
        /// </p>
        /// <p>etc...</p>
        /// <p>
        /// If you want a download box to show up with a pre-populated filename, add this call here 
        /// (supplying a real filename).
        /// </p>
        /// <p>
        /// <code>Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Filename + "\"");</code>
        /// </p>
        /// </remarks>
        /// <param name="context">Context.</param>
        protected abstract void HandleRequest(HttpContext context);

        /// <summary>
        /// Validates the parameters.  Inheriting classes must implement this and return 
        /// true if the parameters are valid, otherwise false.
        /// </summary>
        /// <param name="context">Context.</param>
        /// <returns><c>true</c> if the parameters are valid; otherwise, <c>false</c></returns>
        public virtual bool ValidateParameters(HttpContext context)
        {
            return true;
        }

        /// <summary>
        /// Sets the cache policy.  Unless a handler overrides
        /// this method, handlers will not allow a response to be cached.
        /// </summary>
        /// <param name="cache">Cache.</param>
        public virtual void SetResponseCachePolicy(HttpCachePolicy cache)
        {
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.SetExpires(DateTime.MinValue);
        }

        /// <summary>
        /// Helper method used to Respond to the request that an error occurred in 
        /// processing the request.
        /// </summary>
        /// <param name="context">Context.</param>
        protected void RespondInternalError(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.End();
        }

        /// <summary>
        /// Helper method used to Respond to the request that the request in attempting 
        /// to access a resource that the user does not have access to.
        /// </summary>
        /// <param name="context">Context.</param>
        protected void RespondForbidden(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.End();
        }

        /// <summary>
        /// Helper method used to Respond to the request that the file was not found.
        /// </summary>
        /// <param name="context">Context.</param>
        protected void RespondFileNotFound(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.End();
        }

        protected void WriteOutSimpleTextResponse(string message, HttpContext context)
        {
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ContentType = "text/html";
            context.Response.Write(string.Format("<html><body>{0}</body></html>", message));
        }

        protected void WriteOutFullTextResponse(string message, HttpContext context)
        {
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ContentType = "text/html";
            context.Response.Write(message);
        }

        protected string GetContentTypeFromFileName(string fileName)
        {
            string retVal = string.Empty;

            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".png":
                    retVal = "image/png";
                    break;
                case ".gif":
                    retVal = "image/gif";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                    retVal = "image/jpeg";
                    break;
                case ".doc":
                    retVal = "application/msword";
                    break;
                case ".xls":
                    retVal = "application/vnd.ms-excel";
                    break;
                case ".ppt":
                    retVal = "application/mspowerpoint";
                    break;
                case ".pdf":
                    retVal = "application/pdf";
                    break;
                case ".js":
                    retVal = "text/javascript";
                    break;
                case ".xml":
                    retVal = "application/xml";
                    break;
                case ".txt":
                    retVal = "text/plain";
                    break;
                case ".zip":
                    retVal = "application/zip";
                    break;

                case ".swf":
                    retVal = "application/x-shockwave-flash";
                    break;
                case ".flv":
                    retVal = "video/x-flv";
                    break;

                case ".htm":
                case ".html":
                    retVal = "text/html";
                    break;
                case ".css":
                    retVal = "text/css";
                    break;

                // This was commented out. MOV files may not work with the handler - JHE
                case ".mov":
                    retVal = "video/quicktime";
                    break;

                case ".mpeg":
                case ".mpg":
                    retVal = "video/mpeg";
                    break;

                case ".wmv":
                    retVal = "video/x-ms-wmv";
                    break;

                case ".mp4":
                    retVal = "video/H264";
                    break;

                case ".mp3":
                    retVal = "audio/mpeg";
                    break;

                case ".wav":
                    retVal = "audio/wav";
                    break;

            }

            return retVal;
        }

        protected void AddImageToCache(HttpContext context, Image image, TimeSpan cacheTime)
        {
            context.Cache.Insert(Cache, (Image)image.Clone(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, cacheTime);
        }
        #endregion Methods

        #region QueryString Variable Methods
        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetQueryStringVar<T>(string name)
        {
            return GetQueryStringVar<T>(name, default(T));
        }

        /// <summary>
        /// Currently supports get/set int and string for query Vars - JHE
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetQueryStringVar<T>(string name, T defaultValue)
        {
            Type objectType = typeof(T);
            T result = default(T);
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (HttpContext.Current.Request.QueryString[name] != null)
                    {
                        object obj = (T)result;
                        if (obj is Enum)
                            result = (T)Enum.Parse(objectType, HttpContext.Current.Request.QueryString[name].ToString());
                        else if (obj is Boolean)
                        {
                            string boolValue = HttpContext.Current.Request.QueryString[name].ToString().ToLower();
                            if (boolValue.ToBool())
                                result = (T)Convert.ChangeType(true, objectType);
                            else
                                result = (T)Convert.ChangeType(false, objectType);
                        }
                        else
                            result = (T)Convert.ChangeType(HttpContext.Current.Request.QueryString[name].ToString(), objectType);
                        return result;
                    }
                }
                return defaultValue;
            }
            catch
            {
                // Error Probably caused by bad URL data (returning default data value) - JHE
                return defaultValue;
            }
        }

        public static bool IsQueryStringVarValidInt(string name)
        {
            return GetQueryStringVar<string>(name).IsValidInt();
        }
        #endregion
    }
}
