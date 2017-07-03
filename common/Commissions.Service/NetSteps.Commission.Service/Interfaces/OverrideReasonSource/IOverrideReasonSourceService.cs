using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.OverrideReasonSource
{
	/// <summary>
	/// 
	/// </summary>
	public interface IOverrideReasonSourceService
	{
		/// <summary>
		/// Gets the override reason sources.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IOverrideReasonSource> GetOverrideReasonSources();
	}
}
