
namespace NetSteps.Taxes.Common.Models
{
    public enum JurisdictionLevel
    {
        Apo,
        Borough,
        City,
        Country,
        County,
        District,
        Fpo,
        LocalImprovementDistrict,
        Parish,
        Province,
        SpecialPurposeDistrict,
        State,
        Territory,
        Township,
        TradeBlock,
        TransitDistrict
    }

    public enum TaxApplicabilityKind
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Indicates the tax is applicable to an order/item's price.
        /// </summary>
        Price,
        /// <summary>
        /// Indicates the tax is applicable to an item's shipping cost to buyer.
        /// </summary>
        Shipping,
		/// <summary>
		/// Indicates the tax is applicable to an order's shipping cost.
		/// </summary>
		OrderShipping
    }
}
