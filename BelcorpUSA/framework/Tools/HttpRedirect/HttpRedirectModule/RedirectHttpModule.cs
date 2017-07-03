// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectHttpModule.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Performs the redirect
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HttpRedirectModule
{
    using System.Web;

    using NetSteps.Encore.Core.Wireup;

    /// <summary>
    /// Performs the redirect
    /// </summary>
    public class RedirectHttpModule : IHttpModule
    {
        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes the Program
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void Init(HttpApplication context)
        {
            // Ensure this library is wired up...
            WireupCoordinator.SelfConfigure();

            context.BeginRequest += RedirectHttpRequest.AnalyzeAndRedirect;
        }
    }
}