

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IOptOutRepository
    {
        OptOut Search(string emailAddress);
    }
}
