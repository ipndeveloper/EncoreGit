using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionRewardItemSelection
    {
        int OrderCustomerAccountID { get; set; }
        int ProductID { get; set; }
        decimal? MatchOrderCountFactor { get; set; }
        int? MaxQuantityAffected { get; set; }
        string GeneratedDescription { get; }
    }
}
