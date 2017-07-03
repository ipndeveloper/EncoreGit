using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Model
{
	public interface IPartialCredentials
	{
		string UserUniqueIdentifier { get; set; }
		int SiteID { get; set; }
	}
}
