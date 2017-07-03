using System;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.Tax
{
	/// <summary>
	/// Performs tax-related logic.
	/// </summary>
	[ContractClass(typeof(Contracts.TaxServiceContracts))]
	public interface ITaxService
	{
		void CalculateTax(OrderCustomer customer);
		void CalculateReturnOrderTax(OrderCustomer customer);
		void CalculatePartyTax(Order order);
		void FinalizeTax(OrderCustomer customer);
		void CancelTax(OrderCustomer customer);
		void FinalizePartialReturnTax(OrderCustomer customer);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(ITaxService))]
		internal abstract class TaxServiceContracts : ITaxService
		{
			void ITaxService.CalculateTax(OrderCustomer customer)
			{
				Contract.Requires<ArgumentNullException>(customer != null);
				Contract.Requires<ArgumentNullException>(customer.Order != null);

				throw new NotImplementedException();
			}

			void ITaxService.CalculateReturnOrderTax(OrderCustomer orderCustomer)
			{
				Contract.Requires<ArgumentNullException>(orderCustomer != null);
				Contract.Requires<ArgumentNullException>(orderCustomer.Order != null);
				Contract.Requires<ArgumentNullException>(orderCustomer.Order.OrderTypeID == (short)Constants.OrderType.ReturnOrder);

				throw new NotImplementedException();
			}

			void ITaxService.CalculatePartyTax(Order order)
			{
				Contract.Requires<ArgumentNullException>(order != null);

				throw new NotImplementedException();
			}

			void ITaxService.FinalizeTax(OrderCustomer customer)
			{
				Contract.Requires<ArgumentNullException>(customer != null);
				Contract.Requires<ArgumentNullException>(customer.Order != null);

				throw new NotImplementedException();
			}

			void ITaxService.CancelTax(OrderCustomer customer)
			{
				Contract.Requires<ArgumentNullException>(customer != null);

				throw new NotImplementedException();
			}

			void ITaxService.FinalizePartialReturnTax(OrderCustomer customer)
			{
				Contract.Requires<ArgumentNullException>(customer != null);

				throw new NotImplementedException();
			}
		}
	}
}