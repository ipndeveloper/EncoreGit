using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IReplacementReasonRepository
    {
        List<ReplacementReason> GetAllReasons();
    }
}
