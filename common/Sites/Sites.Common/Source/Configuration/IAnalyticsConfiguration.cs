namespace NetSteps.Sites.Common.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// The AnalyticsConfiguration interface.
    /// </summary>
    public interface IAnalyticsConfiguration
    {
        /// <summary>
        /// Gets the property ids.
        /// </summary>
        ICollection<string> PropertyIds { get; }

        /// <summary>
        /// Gets or sets a value indicating whether is debug.
        /// </summary>
        bool IsDebug { get; set; }
    }
}