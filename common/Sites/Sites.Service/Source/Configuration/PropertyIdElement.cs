namespace NetSteps.Sites.Service.Configuration
{
    using System.Configuration;

    /// <summary>
    /// The property id element.
    /// </summary>
    public class PropertyIdElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the property id.
        /// </summary>
        [ConfigurationProperty("propertyId", IsRequired = true, IsKey = true)]
        public string PropertyId
        {
            get
            {
                return this["propertyId"] as string;
            }
        }
    }
}