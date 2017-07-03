using System.Collections.Generic;
using NetSteps.Data.Common.Context;

namespace NetSteps.OrderAdjustments.Common.Model
{
    /// <summary>
    /// IOrderAdjustment contains the information necessary to record the order adjustment "header" and order and orderline adjustments
    /// </summary>
    public interface IOrderAdjustmentProfile
    {
		/// <summary>
		/// Gets the AccountIDs of the accounts to be affected by this adjustment.
		/// </summary>
		/// <value>
		/// The affected AccountIDs.
		/// </value>
		ICollection<int> AffectedAccountIDs { get; }
		/// <summary>
		/// Gets modifications to be applied to the order or order customer.
		/// </summary>
		/// <value>
		/// The order modifications.
		/// </value>
        IList<IOrderAdjustmentProfileOrderModification> OrderModifications { get; }
		/// <summary>
		/// Gets the order item targeting information necessary to identify the items in the order that are to be adjusted.
		/// </summary>
		/// <value>
		/// The order line modification targets.
		/// </value>
        IList<IOrderAdjustmentProfileOrderItemTarget> OrderLineModificationTargets { get; }
		/// <summary>
		/// Gets steps added to the Order Process added by the OrderAdjustment components.
		/// </summary>
		/// <value>
		/// The added order steps.
		/// </value>
        IList<IOrderAdjustmentOrderStep> AddedOrderSteps { get; }
        string ExtensionProviderKey { get; set; }
		/// <summary>
		/// Gets or sets the description of the OrderAdjustmentProfile (should identify the source).
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
        string Description { get; set; }
    }
}
