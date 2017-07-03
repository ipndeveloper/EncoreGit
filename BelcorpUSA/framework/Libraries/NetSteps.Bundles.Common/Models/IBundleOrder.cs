using System.Collections.Generic;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IBundleOrder
	{
		List<IBundleCustomer> OrderCustomers { get; set; }
	}
}
