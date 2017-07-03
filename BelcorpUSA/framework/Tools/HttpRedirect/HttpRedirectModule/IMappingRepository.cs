// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMappingRepository.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Interface for the Mapping Repository
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HttpRedirectModule
{
    /// <summary>
    /// Interface for the Mapping Repository
    /// </summary>
    public interface IMappingRepository
    {
        /// <summary>
        /// Gets the new site URL
        /// </summary>
        /// <param name="oldSiteName">
        ///  the old site name
        /// </param>
        /// <returns>
        /// returns the new site url as a string
        /// </returns>
        string GetNewSiteURL(string oldSiteName);
    }
}