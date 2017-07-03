using System.Collections.Generic;

namespace NetSteps.Data.Entities.Extensions
{
	public static class TrackableCollectionExtensions
	{
		public static TrackableCollection<T> AddRange<T>(this TrackableCollection<T> list, IEnumerable<T> second)
		{
			foreach (T item in second)
				list.Add(item);
			return list;
		}
	}
}
