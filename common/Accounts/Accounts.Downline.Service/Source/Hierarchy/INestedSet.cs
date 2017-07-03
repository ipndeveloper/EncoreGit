using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Downline.Service.Hierarchy
{
	/// <summary>
	/// If this service is used, then this interface should be moved to NetSteps.Accounts.Downline.Common. - Lundy
	/// </summary>
	public interface INestedSet
	{
		int TreeLevel { get; }
		int LeftAnchor { get; }
		int RightAnchor { get; }
	}
}
