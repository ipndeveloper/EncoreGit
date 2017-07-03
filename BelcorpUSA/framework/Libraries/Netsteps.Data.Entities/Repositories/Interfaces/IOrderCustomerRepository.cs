using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IOrderCustomerRepository
	{
		IList<OrderItem> LoadOrderItems(int orderCustomerID);
		IList<OrderPayment> LoadOrderPayments(int orderCustomerID);
	}
}
