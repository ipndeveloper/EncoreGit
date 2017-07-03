using System;
using System.Data.Objects;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class AccountPaymentMethodRepository
	{
		protected override Func<NetStepsEntities, IQueryable<AccountPaymentMethod>> loadAllFullQuery
		{
			get
			{
				return CompiledQuery.Compile<NetStepsEntities, IQueryable<AccountPaymentMethod>>(
				 (context) => context.AccountPaymentMethods.Include("BillingAddress"));
			}
		}

        public bool IsUsedByAnyActiveOrderTemplates(int accountPaymentMethodID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    return context.OrderPayments
                        .Any(op =>
                            op.SourceAccountPaymentMethodID == accountPaymentMethodID
                            && op.Order.OrderType.IsTemplate
                            // Paid is the only "active" status for an order template
                            && op.Order.OrderStatusID == (short)Constants.OrderStatus.Paid);
                }
            });
        }
	}
}
