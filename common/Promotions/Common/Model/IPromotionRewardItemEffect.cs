using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.Common.Model
{
    public interface IPromotionRewardItemEffect
    {
        string Description { get; set; }
        string Property { get; set; }
        int ModificationOperationID { get; set; }
        decimal? ModificationValue { get; set; }
        string GeneratedDescription { get; }
    }
}
