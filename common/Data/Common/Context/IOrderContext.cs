namespace NetSteps.Data.Common.Context
{
	using System.Collections.Generic;
	using NetSteps.Data.Common.Entities;

	/// <summary>
	/// The OrderContext interface.
	/// </summary>
	public interface IOrderContext
	{
		/// <summary>
		/// Gets or sets the order.
		/// </summary>
		IOrder Order { get; set; }

		/// <summary>
		/// Gets the coupon codes.
		/// </summary>
		IList<ICouponCode> CouponCodes { get; }

        //INI - GR_Encore-07
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        IActivityInfo CurrentActivity { get; set; }
        //FIN - GR_Encore-07

		/// <summary>
		/// Gets the injected order steps.
		/// </summary>
		IList<IOrderStep> InjectedOrderSteps { get; }

		/// <summary>
		/// Gets the valid order status ids for order adjustment.
		/// </summary>
		int[] ValidOrderStatusIdsForOrderAdjustment { get; }

		/// <summary>
		/// Clears the order context.
		/// </summary>
		void Clear();

		/// <summary>
		/// Gets the list of all sorted dynamic kit products
		/// </summary>
		IList<IProduct> SortedDynamicKitProducts { get; }
	}
}
