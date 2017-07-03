using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Data.Common.Services
{
	public interface IInventoryService
	{
		IInventoryProductCheckResponse GetProductAvailabilityForOrder(IOrderContext orderContext, int productID, int quantity);
		IInventoryProductCheckResponse GetProductAvailabilityForOrder(IOrder order, int productID, int quantity);
	}
}
