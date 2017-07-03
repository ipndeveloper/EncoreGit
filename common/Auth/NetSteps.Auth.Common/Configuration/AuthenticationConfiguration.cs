using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Auth.Common.Configuration
{
	public class AuthenticationConfiguration : ConfigurationSection
	{
        [ConfigurationProperty("providers", IsDefaultCollection = false)]
		public ProviderCollection Providers
		{
			get
			{
				return this["providers"] as ProviderCollection;
			}
		}

        [ConfigurationProperty("adminSettings", IsRequired = false)]
        public AdminSettings AdminSettings
        {
            get
            {
                if (this["adminSettings"] != null)
                {
                    return this["adminSettings"] as AdminSettings;
                }
                return new AdminSettings();
            }
        }
	}

}
