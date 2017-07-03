using System;
using System.Collections.Generic;

namespace NetSteps.Promotions.UI.Common.Admin
{
    public interface IPromotionUIModel
    {
        int PromotionId { get; }

        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }

        string Description { get; set; }

        IList<IPromotionUIQualification> Qualifications { get; set; }
        IList<IPromotionUIReward> Rewards { get; set; }
    }
}
