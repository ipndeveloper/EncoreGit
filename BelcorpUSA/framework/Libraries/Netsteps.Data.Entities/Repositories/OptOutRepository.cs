using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class OptOutRepository : IOptOutRepository
    {

        public OptOut Search(string emailAddress)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
                {
                    using (NetStepsEntities context = CreateContext())
                    {
                        return context.OptOuts.FirstOrDefault(o => o.EmailAddress.Equals(emailAddress));
                    }
                });
        }
    }
}
