using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IReplacementReasonBusinessLogic
    {
        List<ReplacementReason> GetAllReasons(Repositories.IReplacementReasonRepository repository);
    }
}
