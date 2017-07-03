namespace NetSteps.Sites.Common.Models
{
    using System.Collections.Generic;

    using NetSteps.Encore.Core.Dto;

    /// <summary>
    /// The settings for a Google Analytics java script snippet.
    /// </summary>
    [DTO]
    public interface ISiteGoogleAnalyticsSettings
    {
        /// <summary>
        /// Gets or sets the property ids.
        /// </summary>
        ICollection<string> PropertyIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in debug mode.
        /// </summary>
        bool IsDebug { get; set; }
    }
}
