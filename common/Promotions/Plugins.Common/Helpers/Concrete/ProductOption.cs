using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Plugins.Common.Helpers.Concrete
{
	[Serializable]
	public class ProductOption : IProductOption
	{
		public int ProductID { get; set; }

		public int Quantity { get; set; }
	}
}
