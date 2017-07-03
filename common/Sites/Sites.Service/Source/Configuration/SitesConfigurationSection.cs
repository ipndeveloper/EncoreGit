using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NetSteps.Sites.Common.Configuration;

namespace NetSteps.Sites.Service.Configuration
{
	/// <summary>
	/// The configuration settings for Sites.
	/// </summary>
	public class SitesConfigurationSection : ConfigurationSection, ISitesConfiguration
	{
		/// <summary>
		/// The SiteTypeID of the current executing site.
		/// </summary>
		[ConfigurationProperty("siteTypeID")]
		public short SiteTypeID
		{
			get
			{
				return (short)this["siteTypeID"];
			}
			set
			{
				this["siteTypeID"] = value;
			}
		}
	}
}
