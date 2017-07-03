using NetSteps.Commissions.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Interfaces.BonusKind
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBonusKindService
	{
		/// <summary>
		/// Gets the bonus kinds.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IBonusKind> GetBonusKinds();

		/// <summary>
		/// Gets the kind of the bonus.
		/// </summary>
		/// <param name="bonusCode">The bonus code.</param>
		/// <returns></returns>
		IBonusKind GetBonusKind(string bonusCode);

		/// <summary>
		/// Gets the kind of the bonus.
		/// </summary>
		/// <param name="bonusKindId">The bonus kind identifier.</param>
		/// <returns></returns>
		IBonusKind GetBonusKind(int bonusKindId);
	}
}
