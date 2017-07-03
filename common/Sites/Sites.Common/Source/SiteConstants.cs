using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common
{
	/// <summary>
	/// Constant values to be used in place of magic numbers and strings.
	/// </summary>
	public static class SiteConstants
	{
		/// <summary>
		/// String values to be used as keys for site settings.
		/// </summary>
		public static class SiteSettingKeys
		{
			/// <summary>
			/// BaseGoogleAnalyticsTrackerID
			/// </summary>
			public static string BaseGoogleAnalyticsTrackerID { get { return "BaseGoogleAnalyticsTrackerID"; } }

			/// <summary>
			/// GoogleAnalyticsTrackerID
			/// </summary>
			public static string GoogleAnalyticsTrackerID { get { return "GoogleAnalyticsTrackerID"; } }
		}
	}
}
