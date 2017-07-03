using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IBundleOrder
	{
		List<IBundleCustomer> OrderCustomers { get; set; }
	}
}
