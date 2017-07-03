using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IOptOutBusinessLogic
    {
        OptOut Search(IOptOutRepository repository, string emailAddress);
    }
}
