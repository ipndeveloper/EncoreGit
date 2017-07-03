using System.Collections.Generic;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IDynamicKitGroup
	{
		int DynamicKitGroupID { get; set; }
		int MinimumProductCount { get; set; }
		int MaximumProductCount { get; set; }
		List<IDynamicKitGroupRule> DynamicKitGroupRules { get; set; }
	}
}
