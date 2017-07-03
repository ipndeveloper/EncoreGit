using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsCore.Areas.Products.Models
{
	public class ProductOverviewModel
	{
		public Product Product { get; set; }

		/// <summary>
		/// The value is a list of productPropertyValues.
		/// iterate through the list of productPropertyValues 
		/// and grab the correct productPropertyValues when needed by productPropertyValueID.
		/// This is because a Product load does not load the productPropertyValues for related variants.
		/// The Key is the ProductID - The Value is the ProductPropertyValues for the ProductID
		/// </summary>
		public Dictionary<int, ProductPropertyValue> VariantProductPropertyValues { get; set; }

		public Dictionary<int, IEnumerable<DescriptionTranslation>> VariantProductTranslations { get; set; }

	}
}