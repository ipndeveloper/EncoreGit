// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingModel.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Defines the MappingModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HttpRedirectModule.Models
{
    /// <summary>
    /// Model for Mapping Redirect Data
    /// </summary>
    public class MappingModel
    {
        /// <summary>
        /// Gets or sets AccountID.
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Gets or sets SiteID.
        /// </summary>
        public int SiteID { get; set; }

        /// <summary>
        /// Gets or sets OldSiteName.
        /// </summary>
        public string OldSiteName { get; set; }

        /// <summary>
        /// Gets or sets NewSiteURL.
        /// </summary>
        public string NewSiteURL { get; set; }
    }
}