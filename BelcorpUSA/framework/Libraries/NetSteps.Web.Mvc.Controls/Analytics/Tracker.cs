namespace NetSteps.Web.Mvc.Controls.Analytics
{
    /// <summary>
    /// The tracker.
    /// </summary>
    public class Tracker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tracker"/> class.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <param name="propertyId">
        /// The property id.
        /// </param>
        public Tracker(string prefix, string propertyId)
        {
            this.Prefix = prefix;
            this.PropertyId = propertyId;
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// Gets the property id.
        /// </summary>
        public string PropertyId { get; private set; }
    }
}