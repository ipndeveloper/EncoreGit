using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sites.Common.Configuration
{
	/// <summary>
	/// The configuration settings for Sites.
	/// </summary>
	public interface ISitesConfiguration
	{
		/// <summary>
		/// The SiteTypeID of the current executing site.
		/// </summary>
		short SiteTypeID { get; }
	}
}
