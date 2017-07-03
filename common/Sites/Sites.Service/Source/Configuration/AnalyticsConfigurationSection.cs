namespace NetSteps.Sites.Service.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The analytics configuration element.
    /// </summary>
    public class AnalyticsConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the property ids.
        /// </summary>
        [ConfigurationProperty("propertyIds")]
        [ConfigurationCollection(typeof(PropertyIdCollection))]
        public PropertyIdCollection PropertyIds
        {
            get
            {
                return (PropertyIdCollection)this["propertyIds"] ??
                   new PropertyIdCollection();
            }
        }

        /// <summary>
        /// Gets a value indicating whether debug.
        /// </summary>
        [ConfigurationProperty("debug", IsRequired = false, DefaultValue = false)]
        public bool Debug
        {
            get
            {
                return (bool?)this["debug"] ?? false;
            }
        }
    }
}
