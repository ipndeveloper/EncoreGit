// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMappingBusinessLogic.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Defines the IMappingBusinessLogic type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HttpRedirectModule
{
    /// <summary>
    /// The interface for mapping business logic
    /// </summary>
    public interface IMappingBusinessLogic
    {
        /// <summary>
        /// Gets the new site URL based on mapping
        /// </summary>
        /// <param name="oldSiteName">
        /// The old site name.
        /// </param>
        /// <returns>
        /// returns a string with the new site name
        /// </returns>
        string GetNewSiteUrl(string oldSiteName);
    }
}