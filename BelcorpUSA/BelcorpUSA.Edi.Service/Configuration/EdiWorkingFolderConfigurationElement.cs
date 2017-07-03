using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Configuration;
using System.Configuration;

namespace BelcorpUSA.Edi.Service.Configuration
{
	public class EdiWorkingFolderConfigurationElement : DeclaredMemberConfigurationElement
	{
		public static readonly ConfigurationProperty LocationProperty = new ConfigurationProperty("location", typeof(string), "%appdata%\busediprocessor");
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
