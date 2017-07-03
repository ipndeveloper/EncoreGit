using System;

namespace NetSteps.Common.Interfaces
{
    public interface ILogger
    {
        void Debug(string msg);
        void Debug(string format, params object[] args);
        void Info(string msg);
        void Info(string format, params object[] args);
        void Error(string msg);
        void Error(string format, params object[] args);
        /// <summary>
        /// Used when the DistributorBackOffice.Controllers.BaseController.OnActionExecuting method intercepts a request for a blacklisted page
        /// </summary>
        /// <param name="url">Requested url</param>
        /// <param name="referringUrl">Referring url</param>
        void AttemptAtBlockedPage(string url, string referringUrl);
    }
}
