using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IApplicationIdentity : IRegisteredIdentity
	{
		string KeyData { get; set; }
	}
}
