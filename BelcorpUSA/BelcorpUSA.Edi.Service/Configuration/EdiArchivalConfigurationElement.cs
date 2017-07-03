using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Configuration;
using System.Configuration;
using System.IO;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiArchivalConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty EnabledProperty = new ConfigurationProperty("enabled", typeof(bool), true);
		public bool Enabled
		{
			get { return (bool)this[EnabledProperty]; }
			set { this[EnabledProperty] = value; }
		}

		public static readonly ConfigurationProperty LocationProperty = new ConfigurationProperty("location", typeof(string), String.Empty);
		public string Location
		{
			get
			{
				string retval = (string)this[LocationProperty];
				if (!String.IsNullOrWhiteSpace(retval))
				{
					bool isUri = Uri.IsWellFormedUriString(retval, UriKind.Absolute);
					if (isUri && !retval.EndsWith("/"))
					{
						retval += "/";
					}
					else if (!isUri && !retval.EndsWith("\\"))
					{
						retval += "\\";
					}
				}
				return retval;
			}
			set { this[LocationProperty] = value; }
		}
	}
}
