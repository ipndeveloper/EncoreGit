using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Services.Override
{
	/// <summary>
	/// 
	/// </summary>
	public interface IOverrideKindService
	{
		/// <summary>
		/// Gets the override kinds.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IOverrideKind> GetOverrideKinds();
	}
}
