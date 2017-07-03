using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class ReplacementReasonBusinessLogic
    {
        public List<ReplacementReason> GetAllReasons(Repositories.IReplacementReasonRepository repository)
        {
            return repository.GetAllReasons();
        }
    }
}
