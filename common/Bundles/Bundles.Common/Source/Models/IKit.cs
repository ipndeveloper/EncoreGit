

namespace NetSteps.Bundles.Common.Models
{
	using System.Collections.Generic;
	using NetSteps.Data.Common.Entities;
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IKit
	{
		List<IBundleItem> OrderItems { get; set; }
		List<IDynamicKit> DynamicKits { get; set; }
		int ProductID { get; set; }
	}
}
