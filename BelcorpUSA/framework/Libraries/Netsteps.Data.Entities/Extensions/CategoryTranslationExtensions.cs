using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: CategoryTranslation Extensions
	/// Created: 06-23-2010
	/// </summary>
	public static class CategoryTranslationExtensions
	{
		public static string Name(this IEnumerable<CategoryTranslation> categoryTranslations)
		{
			var description = GetDescription(categoryTranslations);

			return description == default(CategoryTranslation) ? string.Empty : description.Name;
		}

		public static HtmlContent HtmlContent(this IEnumerable<CategoryTranslation> categoryTranslations)
		{
			var description = GetDescription(categoryTranslations);

			return description == default(CategoryTranslation) ? null : description.HtmlContent;
		}

		private static CategoryTranslation GetDescription(IEnumerable<CategoryTranslation> descriptions)
		{
			if (descriptions.Count() == 0)
				return null;

			var description = descriptions.FirstOrDefault(d => d.LanguageID == ApplicationContext.Instance.CurrentLanguageID);
			return description == default(CategoryTranslation) ? descriptions.FirstOrDefault() : description;
		}
	}
}
