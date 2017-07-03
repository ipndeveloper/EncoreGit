using System;

namespace NetSteps.Taxes.Common.Models
{
  /// <summary>
  /// Common interface for TaxCacheOverride.
  /// 
  /// </summary>
  public interface ITaxCacheOverride
  {
    /// <summary>
    /// The TaxCacheOverrideID for this TaxCacheOverride.
    /// 
    /// </summary>
    int TaxCacheOverrideID { get; set; }

    /// <summary>
    /// The TaxCategoryID for this TaxCacheOverride.
    /// 
    /// </summary>
    int? TaxCategoryID { get; set; }

    /// <summary>
    /// The PostalCode for this TaxCacheOverride.
    /// 
    /// </summary>
    string PostalCode { get; set; }

    /// <summary>
    /// The State for this TaxCacheOverride.
    /// 
    /// </summary>
    string State { get; set; }

    /// <summary>
    /// The StateAbbreviation for this TaxCacheOverride.
    /// 
    /// </summary>
    string StateAbbreviation { get; set; }

    /// <summary>
    /// The City for this TaxCacheOverride.
    /// 
    /// </summary>
    string City { get; set; }

    /// <summary>
    /// The County for this TaxCacheOverride.
    /// 
    /// </summary>
    string County { get; set; }

    /// <summary>
    /// The CountryID for this TaxCacheOverride.
    /// 
    /// </summary>
    int? CountryID { get; set; }

    /// <summary>
    /// The CitySalesTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CitySalesTax { get; set; }

    /// <summary>
    /// The CityUseTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CityUseTax { get; set; }

    /// <summary>
    /// The CityLocalSales for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CityLocalSales { get; set; }

    /// <summary>
    /// The CityLocalUse for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CityLocalUse { get; set; }

    /// <summary>
    /// The CountySalesTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CountySalesTax { get; set; }

    /// <summary>
    /// The CountyUseTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CountyUseTax { get; set; }

    /// <summary>
    /// The CountyLocalSales for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CountyLocalSales { get; set; }

    /// <summary>
    /// The CountyLocalUse for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CountyLocalUse { get; set; }

    /// <summary>
    /// The CountrySalesTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CountrySalesTax { get; set; }

    /// <summary>
    /// The DistrictSalesTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? DistrictSalesTax { get; set; }

    /// <summary>
    /// The StateSalesTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? StateSalesTax { get; set; }

    /// <summary>
    /// The StateUseTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? StateUseTax { get; set; }

    /// <summary>
    /// The CombinedSalesTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CombinedSalesTax { get; set; }

    /// <summary>
    /// The CombinedUseTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? CombinedUseTax { get; set; }

    /// <summary>
    /// The ChargeTaxOnShipping for this TaxCacheOverride.
    /// 
    /// </summary>
    bool? ChargeTaxOnShipping { get; set; }

    /// <summary>
    /// The DateCreatedUTC for this TaxCacheOverride.
    /// 
    /// </summary>
    DateTime DateCreatedUTC { get; set; }

    /// <summary>
    /// The EffectiveDateUTC for this TaxCacheOverride.
    /// 
    /// </summary>
    DateTime EffectiveDateUTC { get; set; }

    /// <summary>
    /// The ExpirationDateUTC for this TaxCacheOverride.
    /// 
    /// </summary>
    DateTime ExpirationDateUTC { get; set; }

    /// <summary>
    /// The DataVersion for this TaxCacheOverride.
    /// 
    /// </summary>
    byte[] DataVersion { get; set; }

    /// <summary>
    /// The SpecialTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? SpecialTax { get; set; }

    /// <summary>
    /// The MiscTax for this TaxCacheOverride.
    /// 
    /// </summary>
    Decimal? MiscTax { get; set; }
  }
}
