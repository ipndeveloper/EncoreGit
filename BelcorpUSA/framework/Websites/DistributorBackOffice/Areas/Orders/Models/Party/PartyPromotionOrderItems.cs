using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Entities;

namespace DistributorBackOffice.Areas.Orders.Models.Party
{
	public class PartyPromotionOrderItems
	{
		public int OrderCustomerAccountID { get; set; }

		public IEnumerable<IGrouping<IOrderAdjustment, IOrderItem>> Items { get; set; }
	}
}