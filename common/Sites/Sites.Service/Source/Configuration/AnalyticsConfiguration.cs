namespace NetSteps.Sites.Service.Configuration
{
    using System.Collections.Generic;

    using NetSteps.Sites.Common.Configuration;

    /// <summary>
    /// The analytics configuration.
    /// </summary>
    public class AnalyticsConfiguration : IAnalyticsConfiguration
    {
        /// <summary>
        /// The analytics configuration section.
        /// </summary>
        private readonly AnalyticsConfigurationSection _analyticsConfigurationSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsConfiguration"/> class.
        /// </summary>
        /// <param name="analyticsConfigurationSection">The analytics configuration section, or null if none was found.</param>
        public AnalyticsConfiguration(AnalyticsConfigurationSection analyticsConfigurationSection)
        {
            _analyticsConfigurationSection = analyticsConfigurationSection;
            PropertyIds = new List<string>();

			// if null, we're done.
			if (analyticsConfigurationSection == null)
			{
				return;
			}

			foreach (PropertyIdElement propertyIdElement in analyticsConfigurationSection.PropertyIds)
            {
                PropertyIds.Add(propertyIdElement.PropertyId);
            }

            IsDebug = analyticsConfigurationSection.Debug;
        }

        /// <summary>
        /// Gets the property ids.
        /// </summary>
        public ICollection<string> PropertyIds { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether is debug.
        /// </summary>
        public bool IsDebug { get; set; }
    }
}