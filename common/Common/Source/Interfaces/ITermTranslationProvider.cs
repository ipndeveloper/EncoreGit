using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Interfaces
{
	/// <summary>
	/// Provides access to term-translations.
	/// </summary>
	public interface ITermTranslationProvider : IExpireCache
	{
		/// <summary>
		/// Returns a localized term using the current language.
		/// </summary>
		/// <param name="termName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		string GetTerm(string termName, string defaultValue);

		/// <summary>
		/// Returns a localized term using the specified language.
		/// </summary>
		/// <param name="languageId"></param>
		/// <param name="termName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		string GetTerm(int languageId, string termName, string defaultValue);

		/// <summary>
		/// Returns a localized term with args applied using the specified language.
		/// </summary>
		/// <param name="languageId"></param>
		/// <param name="termName"></param>
		/// <param name="args"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		string GetTerm(int languageId, string termName, object[] args, string defaultValue = "");
	}
}
