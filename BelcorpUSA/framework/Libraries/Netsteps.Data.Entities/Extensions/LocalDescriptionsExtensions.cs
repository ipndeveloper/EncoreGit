using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: LocalDescriptions Extensions
	/// Created: 06-23-2010
	/// </summary>
	public static class LocalDescriptionsExtensions
	{
		public static string Name(this IEnumerable<DescriptionTranslation> descriptions)
		{
			var description = GetDescription(descriptions);

			return description == null ? string.Empty : description.Name;
		}

		public static string ShortDescription(this IEnumerable<DescriptionTranslation> descriptions)
		{
			var description = GetDescription(descriptions);

			return description == null ? string.Empty : description.DetokenizeShortDescription();
		}

		public static string DetokenizeShortDescription(this DescriptionTranslation description)
		{
			return description == null ? string.Empty : description.ShortDescription.ReplaceCmsTokens();
		}

		public static string LongDescription(this IEnumerable<DescriptionTranslation> descriptions)
		{
			var description = GetDescription(descriptions);

			return description == null ? string.Empty : description.DetokenizeLongDescription();
		}

		public static string DetokenizeLongDescription(this DescriptionTranslation description)
		{
			return description == null ? string.Empty : description.LongDescription.ReplaceCmsTokens();
		}

		private static DescriptionTranslation GetDescription(IEnumerable<DescriptionTranslation> descriptions)
		{
			if (!descriptions.Any())
			{
				return null;
			}

			var description = descriptions.FirstOrDefault(d => d.LanguageID == ApplicationContext.Instance.CurrentLanguageID);
			return description != default(DescriptionTranslation) ? description : descriptions.First();
		}
	}
}
