// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectHttpRequest.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Defines the RedirectHttpRequest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HttpRedirectModule
{
    using System;
    using System.Configuration;
    using System.Web;

    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// Does the HTTP Redirect based on incoming request
    /// </summary>
    public static class RedirectHttpRequest
    {
        /// <summary>
        /// Gets Request.
        /// </summary>
        private static HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary>
        /// Gets Response.
        /// </summary>
        private static HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        /// <summary>
        /// Analyzes the incoming request and performs the redirect
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        public static void AnalyzeAndRedirect(object obj, EventArgs e)
        {
            var defaultWebsite = ConfigurationManager.AppSettings["defaultWebsite"];
            if (defaultWebsite == null)
            {
                return;
            }

            var businessLogic = Create.New<IMappingBusinessLogic>();
            var distributorSiteName = Request.Url.AbsolutePath.Remove(0, 1).Split('/')[0];

            if (!string.IsNullOrEmpty(distributorSiteName))
            {
                var retVal = businessLogic.GetNewSiteUrl(distributorSiteName);
                if (!string.IsNullOrEmpty(retVal))
                {
                    Response.Redirect(retVal);
                }
            }

            Response.Redirect(defaultWebsite);
        }
    }
}