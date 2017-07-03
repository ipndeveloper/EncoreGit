using System;

namespace NetSteps.Promotions.Common.Model
{
    [Flags]
    public enum PromotionStatus
    {
        Enabled = 1,
        Disabled = 1 << 1,
        Obsolete = 1 << 2,
        Archived = 1 << 3,
        Error = 1 << 4
    }
}
