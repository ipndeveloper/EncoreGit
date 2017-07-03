
using NetSteps.Encore.Core.Dto;
namespace NetSteps.Taxes.Common.Models
{
    /// <summary>
    /// An individual tax for an order item.
    /// </summary>
    [DTO]
    public interface ITaxOrderItemTaxes
    {
        /// <summary>
        /// The jurisdiction that imposed the tax.
        /// </summary>
        IJurisdiction Jurisdiction { get; set; }
        /// <summary>
        /// Tax imposition information; usually identifies the specific tax.
        /// </summary>
        string Imposition { get; set; }
        /// <summary>
        /// The taxable amount; price.
        /// </summary>
        decimal TaxableAmount { get; set; }
        /// <summary>
        /// The effective tax rate.
        /// </summary>
        decimal EffectiveRate { get; set; }
        /// <summary>
        /// The calculated tax amount, usually (TaxableAmount * EffectiveRate).
        /// </summary>
        decimal CalculatedTax { get; set; }
        /// <summary>
        /// An identfier for the tax rule imposed.
        /// </summary>
        string TaxRule { get; set; }
    }
}
