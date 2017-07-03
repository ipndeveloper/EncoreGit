using System;

namespace NetSteps.Taxes.Common.Models
{
  /// <summary>
  /// Common interface for AddressTaxOverride.
  /// 
  /// </summary>
  public interface IAddressTaxOverride
  {
    /// <summary>
    /// The AddressTaxOverrideId for this AddressTaxOverride.
    /// 
    /// </summary>
    int AddressTaxOverrideId { get; set; }

    /// <summary>
    /// The AddressID for this AddressTaxOverride.
    /// 
    /// </summary>
    int AddressID { get; set; }

    /// <summary>
    /// The TaxPercent for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercent { get; set; }

    /// <summary>
    /// The TaxPercentCity for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentCity { get; set; }

    /// <summary>
    /// The TaxPercentState for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentState { get; set; }

    /// <summary>
    /// The TaxPercentCounty for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentCounty { get; set; }

    /// <summary>
    /// The TaxPercentDistrict for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentDistrict { get; set; }

    /// <summary>
    /// The TaxPercentCountry for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentCountry { get; set; }

    /// <summary>
    /// The TaxPercentSpecial for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentSpecial { get; set; }

    /// <summary>
    /// The TaxPercentMisc for this AddressTaxOverride.
    /// 
    /// </summary>
    Decimal? TaxPercentMisc { get; set; }

    /// <summary>
    /// The DateCreatedUTC for this AddressTaxOverride.
    /// 
    /// </summary>
    DateTime? DateCreatedUTC { get; set; }

    /// <summary>
    /// The ExpirationDateUTC for this AddressTaxOverride.
    /// 
    /// </summary>
    DateTime? ExpirationDateUTC { get; set; }
  }
}
