namespace NetSteps.Data.Common.Locale
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// The i language id extensions.
	/// </summary>
	public static class ILanguageIDExtensions
	{
		/// <summary>
		/// The get by language id or default for display.
		/// </summary>
		/// <param name="descriptionTranslations">
		/// The description translations.
		/// </param>
		/// <param name="languageID">
		/// The language id.
		/// </param>
		/// <typeparam name="T">
		/// </typeparam>
		/// <returns>
		/// The type that implements ILanguageID.
		/// </returns>
		public static T GetByLanguageIdOrDefaultForDisplay<T>(this ICollection<T> descriptionTranslations, int languageID) where T : ILanguageID
		{
			T result = descriptionTranslations.FirstOrDefault(d => d.LanguageID == languageID);

			if (result == null)
			{
				if (descriptionTranslations.Any())
				{
					result = descriptionTranslations.FirstOrDefault();
				}
			}

			return result;
		}
	}
}
