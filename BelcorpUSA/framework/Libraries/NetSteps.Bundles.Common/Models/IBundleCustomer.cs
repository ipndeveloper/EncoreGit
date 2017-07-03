using System.Collections.Generic;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IBundleCustomer
	{
		int AccountID { get; set; }
		List<IBundleItem> OrderItems { get; set; }
	}
}
