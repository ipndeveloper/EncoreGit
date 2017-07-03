using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.TitleKind
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITitleKindService
	{
		/// <summary>
		/// Gets the title kinds.
		/// </summary>
		/// <returns></returns>
		IEnumerable<ITitleKind> GetTitleKinds();
		/// <summary>
		/// Gets the kind of the title.
		/// </summary>
		/// <param name="titleKindId">The title kind identifier.</param>
		/// <returns></returns>
		ITitleKind GetTitleKind(int titleKindId);
	}
}
