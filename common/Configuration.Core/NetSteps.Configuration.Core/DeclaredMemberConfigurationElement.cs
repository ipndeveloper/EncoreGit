using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Configuration
{
	public abstract class DeclaredMemberConfigurationElement : ConfigurationElement
	{
		protected virtual void DoInitializeProperties()
		{
			this.InitializeProperties(this.Properties);
		}

		public DeclaredMemberConfigurationElement()
		{
			DoInitializeProperties();
		}
	}
}
