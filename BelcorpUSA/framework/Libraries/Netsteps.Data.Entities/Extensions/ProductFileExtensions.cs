using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Extensions
{
	/// <summary>
	/// Author: John Egbert
	/// Description: ProductFile Extensions
	/// Created: 06-23-2010
	/// </summary>
	public static class ProductFileExtensions
	{
		public static List<ProductFile> GetByProductFileTypeID(this IEnumerable<ProductFile> productFiles, int productFileTypeID)
		{
			return productFiles.Where(p => p.ProductFileTypeID == productFileTypeID).ToList();
		}

		public static bool ContainsProductFileTypeID(this IEnumerable<ProductFile> productFiles, int productFileTypeID)
		{
			return !productFiles.GetByProductFileTypeID(productFileTypeID).IsNullOrEmpty();
		}

		public static int GetNextSortIndex(this IEnumerable<ProductFile> productFiles, int productFileTypeID)
		{
			var files = productFiles.GetByProductFileTypeID(productFileTypeID);
			return !files.IsNullOrEmpty() ? files.Max(f => f.SortIndex) + 1 : 0;
		}

	}
}
