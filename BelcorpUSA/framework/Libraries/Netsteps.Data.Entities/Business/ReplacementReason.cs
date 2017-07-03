using System.Collections.Generic;

namespace NetSteps.Data.Entities
{
    public partial class ReplacementReason
    {
        public static List<ReplacementReason> GetAllReasons()
        {
            return BusinessLogic.GetAllReasons(Repository);
        }
    }
}
