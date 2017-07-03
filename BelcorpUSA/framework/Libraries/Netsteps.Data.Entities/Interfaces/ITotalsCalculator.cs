using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Entities.Interfaces
{
	[ContractClass(typeof(Contracts.TotalsCalculatorContracts))]
	public interface ITotalsCalculator
	{
		//Added an override accountTypeID that can be passed by business logic to determine how to calculate item prices
		void CalculateOrder(Order order, short? accountTypeIDToUseForCalculations = null);
		decimal SumHostessRewards(Order order);
		decimal SumCompletedOrderPayments(Order order);

		//void CalculateOrderBalances(Order order);
		//void ResetOrderCustomerTotals(OrderCustomer orderCustomer);
		//Added an override accountTypeID that can be passed by business logic to determine how to calculate item prices
		//void CalculateOrderCustomerTotals(OrderCustomer orderCustomer, short? accountTypeIDToUseForCalculations = null);
		//void CalculateOrderCustomerBalances(OrderCustomer orderCustomer);
		//Added an override accountTypeID that can be passed by business logic to determine how to calculate item prices
		//void CalculateOrderCustomerProductTotals(OrderCustomer orderCustomer, short? accountTypeIDToUseForCalculations = null);
		//void SetItemPrice(OrderItem orderItem, Product product, int quantity, OrderCustomer orderCustomer, decimal itemPriceOverride, decimal itemCVOverride, bool wholesaleOverride, int accountTypeIDOverride = 0);
		//void ResetOrderTotals(Order order);
		//void CalculateHostessRewards(Order order);
		//void RedeemHostReward(OrderItem orderItem, int? hostRewardRuleId = null);
		//void OverrideCommissionOrderTotals(OrderCustomer order);
		//void FinalizeTax(Order order);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(ITotalsCalculator))]
		internal abstract class TotalsCalculatorContracts : ITotalsCalculator
		{
			public void CalculateOrder(Order order, short? accountTypeIDToUseForCalculations = new short?())
			{
				Contract.Requires(order != null);

				throw new System.NotImplementedException();
			}

			public decimal SumHostessRewards(Order order)
			{
				Contract.Requires(order != null);

				throw new System.NotImplementedException();
			}

			public decimal SumCompletedOrderPayments(Order order)
			{
				Contract.Requires<ArgumentNullException>(order != null);

				throw new System.NotImplementedException();
			}
		}
	}
}
