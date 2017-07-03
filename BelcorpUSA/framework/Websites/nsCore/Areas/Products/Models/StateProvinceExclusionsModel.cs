using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Products.Models
{
	public class StateProvinceExclusionsModel
	{
		public Product Product { get; set; }

		public IDictionary<Country, IEnumerable<StateProvince>> AvailableStateProvinces { get; set; }

		public IEnumerable<int> ExistingExcludedStateProvinceIDs { get; set; }
	}
}