using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public interface IPassword
	{
		string Value { get; set; }

		DateTime ValidDate { get; set; }

		IUserIdentity User { get; set; }

		IPasswordPolicy AppliedPolicy { get; }
	}
}
