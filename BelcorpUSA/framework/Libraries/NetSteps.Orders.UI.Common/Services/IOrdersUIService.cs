using NetSteps.Orders.UI.Common.Models;

namespace NetSteps.Orders.UI.Common.Services
{
	public interface IOrdersUIService
	{
		IOrderDetailListModel GetOrderDetailModelsByOrderID(int orderID);
	}
}
