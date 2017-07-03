using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
	public static class IEnumerableExtensions
	{
		public static TrackableCollection<T> ToTrackableCollection<T>(this IEnumerable<T> list)
		{
			TrackableCollection<T> trackableCollection = new TrackableCollection<T>();
			return trackableCollection.AddRange(list);
		}

		public static Category FindCategory(this IEnumerable<Category> categories, int categoryID)
		{
			var category = categories.FirstOrDefault(c => c.CategoryID == categoryID);
			if (category != default(Category))
				return category;

			foreach (Category cat in categories)
			{
				if (cat.ChildCategories != null && cat.ChildCategories.Count > 0)
				{
					category = cat.ChildCategories.FindCategory(categoryID);
					if (category != default(Category))
						return category;
				}
			}

			return null;
		}
	}
}
