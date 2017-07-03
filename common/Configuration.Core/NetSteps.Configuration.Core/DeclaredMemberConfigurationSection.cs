using System;
using System.Configuration;
using System.Reflection;

namespace NetSteps.Configuration
{
	public abstract class DeclaredMemberConfigurationSection : ConfigurationSection
	{
		protected virtual void DoInitializeProperties()
		{
			this.InitializeProperties(this.Properties);
		}

		public DeclaredMemberConfigurationSection()
		{
			DoInitializeProperties();
		}
	}
}
