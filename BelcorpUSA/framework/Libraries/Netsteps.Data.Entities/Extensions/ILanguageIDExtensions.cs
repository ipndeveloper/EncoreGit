using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: ILanguageID Extensions
	/// Created: 07-08-2010
	/// </summary>
	public static class ILanguageIDExtensions
	{
		public static bool ContainsLanguageID<T>(this IEnumerable<T> categoryTranslations, int languageID) where T : ILanguageID
		{
			return categoryTranslations.Count(t => t.LanguageID == languageID) > 0;
		}

		public static T GetByLanguageID<T>(this TrackableCollection<T> descriptionTranslations, int languageID) where T : ILanguageID
		{
			return descriptionTranslations.FirstOrDefault(d => d.LanguageID == languageID);
		}

		/// <summary>
		/// Finds the element in the list with a matching LanguageID or returns a new object with that LanguageID set. - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="descriptionTranslations"></param>
		/// <param name="languageID"></param>
		/// <returns></returns>
		public static T GetByLanguageIdOrDefault<T>(this TrackableCollection<T> descriptionTranslations, int languageID = 0) where T : ILanguageID, new()
		{
			if (languageID == 0)
			{
				languageID = ApplicationContext.Instance.CurrentLanguageID;
			}
			T result = descriptionTranslations.FirstOrDefault(d => d.LanguageID == languageID);
			if (result == null)
			{
				result = new T { LanguageID = languageID };
			}
			return result;
		}

		/// <summary>
		/// Finds the element in the list with a matching LanguageID or returns a new object with that LanguageID set and adds it to the collection. - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="descriptionTranslations"></param>
		/// <param name="languageID"></param>
		/// <returns></returns>
		public static T GetByLanguageIdOrDefaultInList<T>(this TrackableCollection<T> descriptionTranslations, int languageID = 0) where T : ILanguageID, new()
		{
			if (languageID == 0)
			{
				languageID = ApplicationContext.Instance.CurrentLanguageID;
			}
			T result = descriptionTranslations.FirstOrDefault(d => d.LanguageID == languageID);
			if (result == null)
			{
				result = new T { LanguageID = languageID };
				descriptionTranslations.Add(result);
			}
			return result;
		}

		public static T GetByLanguageIdOrDefaultForDisplay<T>(this TrackableCollection<T> descriptionTranslations, int languageID = 0) where T : ILanguageID, new()
		{
			if (languageID == 0)
			{
				languageID = ApplicationContext.Instance.CurrentLanguageID;
			}
			T result = descriptionTranslations.FirstOrDefault(d => d.LanguageID == languageID);
			if (result == null)
			{
				if (descriptionTranslations.Any())
				{
					result = descriptionTranslations.FirstOrDefault();
				}
				else
				{
					result = new T { LanguageID = languageID };
				}
			}
			return result;
		}
	}
}