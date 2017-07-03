using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Auth.Common.Model;

namespace NetSteps.Auth.Service.Model.Concrete
{
	public class AuthenticationConfiguration : IAuthenticationConfiguration
	{
		public IEnumerable<string> RegisteredProviders { get; internal set; }
		public IDictionary<string, bool> AdminSettings { get; internal set; }
	}
}
