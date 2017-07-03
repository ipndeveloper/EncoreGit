using System.Collections.Generic;

namespace NetSteps.Data.Common.Entities
{
	public interface IOrderCustomer
	{
		IList<IOrderItem> OrderItems { get; }
		IEnumerable<IOrderItem> AdjustableOrderItems { get; }

		IList<IOrderAdjustmentOrderModification> OrderModifications { get; }

		short AccountTypeID { get; }

		int AccountID { get; set; }

		void AddOrderModification(IOrderAdjustmentOrderModification modification);

		decimal AdjustedShippingTotal { get; }
		decimal AdjustedTaxTotal { get; }
		decimal AdjustedHandlingTotal { get; }
		decimal ShippingAdjustmentAmount { get; }
		decimal AdjustedSubTotal { get; }
		decimal ProductSubTotal { get; }
		short OrderCustomerTypeID { get; }
	}
}
