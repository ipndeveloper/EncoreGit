using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Bundles.Common.Models
{
	using NetSteps.Encore.Core.Dto;

	[DTO]
	public interface IDynamicKitGroupRule
	{
		int ProductID { get; set; }
		int ProductTypeID { get; set; }
		int DynamicKitGroupRuleID { get; set; }
		bool Default { get; set; }
		bool Include { get; set; }
		bool Required { get; set; }
		int SortOrder { get; set; }
	}
}
