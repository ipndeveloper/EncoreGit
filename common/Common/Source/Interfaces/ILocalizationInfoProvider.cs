using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Models;

namespace NetSteps.Common.Interfaces
{
	/// <summary>
	/// Provides the proper localization content per client
	/// </summary>
	public interface ILocalizationInfoProvider
	{
		/// <summary>
		/// Returns you the localized info object.
		/// </summary>
		/// <returns></returns>
		ILocalizationInfo GetLocalizationInfo();
	}
}
