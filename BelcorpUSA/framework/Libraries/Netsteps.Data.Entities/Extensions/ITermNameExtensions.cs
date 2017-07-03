using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: ITermName Extensions
	/// Created: 06-30-2010
	/// </summary>
	public static class ITermNameExtensions
	{
		/// <summary>
		/// Returns the Translated term according to the current User.LanguageID in the ApplicationContext and 
		/// TermName, Name properties on the Entity. - JHE
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string GetTerm(this ITermName obj)
		{
			return obj == null ? string.Empty : (!obj.TermName.IsNullOrEmpty()) ? CachedData.Translation.GetTerm(obj.TermName, obj.Name) : obj.Name;
		}

		public static string GetTerm(this ITermName obj, int languageId)
		{
			return obj == null ? string.Empty : (!obj.TermName.IsNullOrEmpty()) ? CachedData.Translation.GetTerm(languageId, obj.TermName, obj.Name) : obj.Name;
		}
	}
}
