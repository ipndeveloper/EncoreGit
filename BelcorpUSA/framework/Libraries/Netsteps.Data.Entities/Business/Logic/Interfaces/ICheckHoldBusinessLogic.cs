using NetSteps.Data.Entities.Commissions;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface ICheckHoldBusinessLogic
    {
        void InsertOrUpdate(ICheckHoldRepository repository, CheckHold checkHold);
    }
}
