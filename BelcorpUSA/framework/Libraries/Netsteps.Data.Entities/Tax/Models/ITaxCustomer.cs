using System.Collections.Generic;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{    
    [DTO]
    public interface ITaxCustomer
    {
		/// <summary>
		/// CustomerId for identifying in host system (OrderCustomer.OrderCustomerID)
		/// </summary>
        string CustomerID { get; set; }

		/// <summary>
		/// Whether tax should be charged for this customer.
		/// </summary>
        bool IsTaxExempt { get; set; }

		/// <summary>
		/// Line items for this customer.
		/// </summary>
		List<ITaxOrderItem> Items { get; set; }

		/// <summary>
		/// The customer's shipping address, if applicable.
		/// </summary>
		ITaxAddress ShippingAddress { get; set; }
	}
}
