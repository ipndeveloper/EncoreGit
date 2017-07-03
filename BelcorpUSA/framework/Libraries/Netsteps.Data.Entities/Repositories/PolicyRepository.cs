using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class PolicyRepository
    {
        #region Methods
        public List<Policy> GetPolicies(int accountTypeID, int marketID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var policies = context.Policies
                        .Include("HtmlSection.HtmlSectionContents.HtmlContent.HtmlElements")
                        .Where(p => p.AccountTypeID == accountTypeID && p.Markets.Any(m => m.MarketID == marketID))
                        .OrderBy(p => p.DateReleasedUTC);
					
                    return policies.ToList();
                }
            });
        }
        #endregion
    }
}
