using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nsCore.Areas.Products.Models.Promotions.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Dto;

namespace nsCore.Areas.Products.Models.Promotions
{
	[DTO]
    public interface ICustomerQVRangeCartCondition : ICartConditionModel
    {
		decimal? QVMinimumSubtotal { get; set; }
		decimal? QVMaximumSubtotal { get; set; }
		int PriceTypeID { get; set; }
        bool Cumulative { get; set; }
    }
}