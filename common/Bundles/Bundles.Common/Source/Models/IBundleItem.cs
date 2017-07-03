using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IBundleItem
	{
		int ProductID { get; set; }
		int ProductTypeID { get; set; }
		int Quantity { get; set; }
		bool IsHostReward { get; set; }
		bool IsDynamicKit { get; set; }
		bool IsStaticKit { get; set; }
		bool IsParentStaticKit { get; set; }
		bool HasAdjustmentOrderLineModifications { get; set; }
	}
}
