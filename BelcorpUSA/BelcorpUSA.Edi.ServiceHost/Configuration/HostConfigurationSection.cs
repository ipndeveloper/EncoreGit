using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Configuration;
using System.Configuration;

namespace BelcorpUSA.Edi.ServiceHost.Configuration
{
	public class HostConfigurationSection : DeclaredMemberConfigurationSection
	{
		public static readonly ConfigurationProperty ServiceExecutionIntervalProperty = new ConfigurationProperty("serviceExecutionInterval", typeof(TimeSpan), "0.00:15:00.0");
		public TimeSpan ServiceExecutionInterval
		{
			get { return (TimeSpan)this[ServiceExecutionIntervalProperty]; }
			set { this[ServiceExecutionIntervalProperty] = value; }
		}

		public static readonly ConfigurationProperty alignServiceExecutionToClockProperty = new ConfigurationProperty("alignServiceExecutionToClock", typeof(bool), true);
		public bool AlignServiceExecutionToClock
		{
			get { return (bool)this[alignServiceExecutionToClockProperty]; }
			set { this[alignServiceExecutionToClockProperty] = value; }
		}

		public static readonly ConfigurationProperty ExecuteImmediatelyProperty = new ConfigurationProperty("executeImmediately", typeof(bool), false);
		public bool ExecuteImmediately
		{
			get { return (bool)this[ExecuteImmediatelyProperty]; }
			set { this[ExecuteImmediatelyProperty] = value; }
		}
	}
}
