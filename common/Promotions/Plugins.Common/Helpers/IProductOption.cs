using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Plugins.Common.Helpers
{
	public interface IProductOption
	{
		int ProductID { get; set; }
		int Quantity { get; set; }
	}
}
