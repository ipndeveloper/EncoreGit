using System;

namespace NetSteps.Taxes.Common.Models
{
  /// <summary>
  /// Common interface for TaxCache.
  /// 
  /// </summary>
  public interface ITaxCache
  {
    /// <summary>
    /// The TaxCacheID for this TaxCache.
    /// 
    /// </summary>
    int TaxCacheID { get; set; }

    /// <summary>
    /// The TaxDataSourceID for this TaxCache.
    /// 
    /// </summary>
    short? TaxDataSourceID { get; set; }

    /// <summary>
    /// The WarehouseAddressID for this TaxCache.
    /// 
    /// </summary>
    int? WarehouseAddressID { get; set; }

    /// <summary>
    /// The TaxCategoryID for this TaxCache.
    /// 
    /// </summary>
    int? TaxCategoryID { get; set; }

    /// <summary>
    /// The PostalCode for this TaxCache.
    /// 
    /// </summary>
    string PostalCode { get; set; }

    /// <summary>
    /// The State for this TaxCache.
    /// 
    /// </summary>
    string State { get; set; }

    /// <summary>
    /// The StateAbbreviation for this TaxCache.
    /// 
    /// </summary>
    string StateAbbreviation { get; set; }

    /// <summary>
    /// The City for this TaxCache.
    /// 
    /// </summary>
    string City { get; set; }

    /// <summary>
    /// The County for this TaxCache.
    /// 
    /// </summary>
    string County { get; set; }

    /// <summary>
    /// The CountryID for this TaxCache.
    /// 
    /// </summary>
    int? CountryID { get; set; }

    /// <summary>
    /// The CountyFIPS for this TaxCache.
    /// 
    /// </summary>
    string CountyFIPS { get; set; }

    /// <summary>
    /// The CitySalesTax for this TaxCache.
    /// 
    /// </summary>
    Decimal CitySalesTax { get; set; }

    /// <summary>
    /// The CityUseTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? CityUseTax { get; set; }

    /// <summary>
    /// The CityLocalSales for this TaxCache.
    /// 
    /// </summary>
    Decimal? CityLocalSales { get; set; }

    /// <summary>
    /// The CityLocalUse for this TaxCache.
    /// 
    /// </summary>
    Decimal? CityLocalUse { get; set; }

    /// <summary>
    /// The CountySalesTax for this TaxCache.
    /// 
    /// </summary>
    Decimal CountySalesTax { get; set; }

    /// <summary>
    /// The CountyUseTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? CountyUseTax { get; set; }

    /// <summary>
    /// The CountyLocalSales for this TaxCache.
    /// 
    /// </summary>
    Decimal? CountyLocalSales { get; set; }

    /// <summary>
    /// The CountyLocalUse for this TaxCache.
    /// 
    /// </summary>
    Decimal? CountyLocalUse { get; set; }

    /// <summary>
    /// The CountrySalesTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? CountrySalesTax { get; set; }

    /// <summary>
    /// The DistrictSalesTax for this TaxCache.
    /// 
    /// </summary>
    Decimal DistrictSalesTax { get; set; }

    /// <summary>
    /// The StateSalesTax for this TaxCache.
    /// 
    /// </summary>
    Decimal StateSalesTax { get; set; }

    /// <summary>
    /// The StateUseTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? StateUseTax { get; set; }

    /// <summary>
    /// The CombinedSalesTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? CombinedSalesTax { get; set; }

    /// <summary>
    /// The CombinedUseTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? CombinedUseTax { get; set; }

    /// <summary>
    /// The CountyDefault for this TaxCache.
    /// 
    /// </summary>
    bool? CountyDefault { get; set; }

    /// <summary>
    /// The GeneralDefault for this TaxCache.
    /// 
    /// </summary>
    bool? GeneralDefault { get; set; }

    /// <summary>
    /// The InCityLimits for this TaxCache.
    /// 
    /// </summary>
    bool? InCityLimits { get; set; }

    /// <summary>
    /// The ChargeTaxOnShipping for this TaxCache.
    /// 
    /// </summary>
    bool ChargeTaxOnShipping { get; set; }

    /// <summary>
    /// The DateCreatedUTC for this TaxCache.
    /// 
    /// </summary>
    DateTime DateCreatedUTC { get; set; }

    /// <summary>
    /// The DateCachedUTC for this TaxCache.
    /// 
    /// </summary>
    DateTime DateCachedUTC { get; set; }

    /// <summary>
    /// The EffectiveDateUTC for this TaxCache.
    /// 
    /// </summary>
    DateTime? EffectiveDateUTC { get; set; }

    /// <summary>
    /// The ExpirationDateUTC for this TaxCache.
    /// 
    /// </summary>
    DateTime? ExpirationDateUTC { get; set; }

    /// <summary>
    /// The Latitude for this TaxCache.
    /// 
    /// </summary>
    double? Latitude { get; set; }

    /// <summary>
    /// The Longitude for this TaxCache.
    /// 
    /// </summary>
    double? Longitude { get; set; }

    /// <summary>
    /// The DataVersion for this TaxCache.
    /// 
    /// </summary>
    byte[] DataVersion { get; set; }

    /// <summary>
    /// The SpecialTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? SpecialTax { get; set; }

    /// <summary>
    /// The MiscTax for this TaxCache.
    /// 
    /// </summary>
    Decimal? MiscTax { get; set; }

    /// <summary>
    /// The TaxPercentage for this TaxCache.
    /// 
    /// </summary>
    Decimal? TaxPercentage { get; set; }
  }
}
