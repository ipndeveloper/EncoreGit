using System.Collections.Generic;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IDynamicKit
	{
		int DynamicKitID { get; set; }
		int DynamicKitProductID { get; set; }
		List<IDynamicKitGroup> DynamicKitGroups { get; set; }
	}
}
