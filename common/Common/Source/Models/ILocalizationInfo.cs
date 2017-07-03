using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Defines localization information for accessing localized content.
	/// </summary>
	[DTO]
	public interface ILocalizationInfo
	{
		/// <summary>
		/// A predefined <see cref="System.Globalization.CultureInfo"/> name. CultureName is not case-sensitive.
		/// </summary>
		string CultureName { get; set; }

		/// <summary>
		/// The LanguageId key in the Core database.
		/// </summary>
		int LanguageId { get; set; }
	}
}
