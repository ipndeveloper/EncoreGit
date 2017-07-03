using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Modules.Order.Common.Models;

namespace NetSteps.Modules.Order.Common.Results
{
    /// <summary>
    /// Order Create Result
    /// </summary>
    [DTO]
    public interface IOrderCreateResult : IResult
    {
        /// <summary>
        /// OrderID
        /// </summary>
        int OrderID { get; set; }
		/// <summary>
		/// List of available ShippingMethods
		/// </summary>
		List<IShippingMethod> ShippingMethods { get; set; }
		/// <summary>
		/// OrderItems
		/// </summary>
		List<IOrderItem> OrderItems { get; set; }
		/// <summary>
		/// Subtotal
		/// </summary>
		decimal Subtotal { get; set; }
		/// <summary>
		/// Tax
		/// </summary>
		decimal Tax { get; set; }
		/// <summary>
		/// Shipping
		/// </summary>
		decimal Shipping { get; set; }
		/// <summary>
		/// GrandTotal
		/// </summary>
		decimal GrandTotal { get; set; }
		/// <summary>
		/// Final ShippingMethod used
		/// </summary>
		IShippingMethod ShippingMethod { get; set; }
    }
}
