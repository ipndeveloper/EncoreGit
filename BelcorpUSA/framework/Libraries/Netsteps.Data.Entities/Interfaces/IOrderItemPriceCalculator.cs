// -----------------------------------------------------------------------
// <copyright file="IOrderItemPriceCalculator.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.Data.Entities.Interfaces
{

    /// <summary>
	/// TODO: Update summary.
	/// </summary>
	public interface IOrderItemPriceCalculator
	{
		void SetOrderItemPrices(OrderCustomer orderCustomer, short? accountTypeIDToUseForCalculations = null);

	}
}
