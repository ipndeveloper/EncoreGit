using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
	public partial class OrderCustomerRepository
	{
		public IList<OrderItem> LoadOrderItems(int orderCustomerID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.OrderItems
						.Where(oi => oi.OrderCustomerID == orderCustomerID)
						.OrderByDescending(oi => oi.OrderCustomerID)
						.ToList();
				}
			});
		}

		public IList<OrderPayment> LoadOrderPayments(int orderCustomerID)
		{
			return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
			{
				using (NetStepsEntities context = CreateContext())
				{
					return context.OrderPayments
						.Where(oi => oi.OrderCustomerID == orderCustomerID)
						.OrderByDescending(oi => oi.OrderCustomerID)
						.ToList();
				}
			});
		}
	}
}
