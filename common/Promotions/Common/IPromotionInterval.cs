using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Common
{
	[DTO]
	public interface IPromotionInterval
	{
		DateTime? StartDate { get; set; }
		DateTime? EndDate { get; set; }
	}
}
