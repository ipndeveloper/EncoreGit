using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Auth.Common.Configuration
{
	public class ProviderElement : ConfigurationElement
	{
		[ConfigurationProperty("name",IsRequired=true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
		}
	}
}
