using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class RedemptionRepository
    {
        public Redemption Load(string redemptionNumber)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.Redemptions
                        .Include("PublicationChannels")
                        .Include("RedemptionMethod").
                        Where(x => x.RedemptionNumber == redemptionNumber).FirstOrDefault();
                }
            });
        }
    }
}
