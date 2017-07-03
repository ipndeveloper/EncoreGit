namespace NetSteps.Web.Mvc.Controls.Analytics
{
    using System.Collections.Generic;
    using System.Web;

    using NetSteps.Sites.Common.Models;

    /// <summary>
    /// The analytics model.
    /// </summary>
    public class AnalyticsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyticsModel"/> class.
        /// </summary>
        /// <param name="httpRequest">
        /// The http Request.
        /// </param>
        public AnalyticsModel(HttpRequestBase httpRequest)
        {
            IEnumerable<string> propertyIds = new List<string>();
            this.FilePath = "/ga.js";

            if (httpRequest.RequestContext != null && httpRequest.RequestContext.HttpContext != null)
            {
                var siteSettings = httpRequest.RequestContext.HttpContext.Items["CurrentSiteAnalyticsSettings"] as ISiteGoogleAnalyticsSettings;
                if (siteSettings != null)
                {
                    propertyIds = siteSettings.PropertyIds;
                    if (siteSettings.IsDebug)
                    {
                        this.FilePath = "/u/ga_debug.js";
                    }
                }
            }

            var trackerPrefixNumber = 0;
            this.Trackers = new List<Tracker>();
            foreach (var propertyId in propertyIds)
            {
                var trackerPrefix = string.Empty;
                if (trackerPrefixNumber > 0)
                {
                    trackerPrefix = string.Concat((char)trackerPrefixNumber, ".");
                }

                this.Trackers.Add(new Tracker(trackerPrefix, propertyId));

                if (trackerPrefixNumber == 0)
                {
                    trackerPrefixNumber = 98;
                }
                else
                {
                    trackerPrefixNumber += 1;
                }
            }

            if (this.Trackers.Count > 0)
            {
                this.AnalyticsEnabled = true;
            }

            if (httpRequest.Url != null)
            {
                this.HostName = httpRequest.Url.Authority;
            }
        }

        /// <summary>
        /// Gets the trackers.
        /// </summary>
        public List<Tracker> Trackers { get; private set; }

        /// <summary>
        /// Gets a value indicating whether analytics enabled.
        /// </summary>
        public bool AnalyticsEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the host name.
        /// </summary>
        public string HostName { get; set; }
    }
}