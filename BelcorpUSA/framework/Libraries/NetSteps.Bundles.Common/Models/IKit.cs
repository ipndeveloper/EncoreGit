using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Bundles.Common.Models
{
	[DTO]
	public interface IKit
	{
		List<IBundleItem> OrderItems { get; set; }
		List<IDynamicKit> DynamicKits { get; set; }
		int ProductID { get; set; }
	}
}
