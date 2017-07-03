// -----------------------------------------------------------------------
// <copyright file="IPaymentCommissions.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ContractClass(typeof(Contracts.PaymentCommissionsContracts))]
    public interface IPaymentCommissions
    {
        void CalculateCommissions(Order order);
    }

	namespace Contracts
	{
		[ContractClassFor(typeof(IPaymentCommissions))]
		abstract class PaymentCommissionsContracts : IPaymentCommissions
		{
			public void CalculateCommissions(Order order)
			{
				Contract.Requires<ArgumentNullException>(order != null);
				Contract.Requires<ArgumentNullException>(order.OrderCustomers != null);
				Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

				throw new NotImplementedException();
			}
		}
	}
}