using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Commissions;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Override Reason Extensions
    /// Created: 11-02-2010
    /// </summary>
    public static class OverrideReasonExtensions
    {
        private static int _defaultOverrideReasonSourceID = 1;

        public static List<OverrideReason> GetOverrideReasons(this IEnumerable<OverrideReason> overrideReasons)
        {
            return overrideReasons.GetOverrideReasons(_defaultOverrideReasonSourceID);
        }

        public static List<OverrideReason> GetOverrideReasons(this IEnumerable<OverrideReason> overrideReasons, int overrideReasonSourceID)
        {
            return overrideReasons.Where(p => p.OverrideReasonSourceID == overrideReasonSourceID).ToList();
        }
    }
}
