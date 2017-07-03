using NetSteps.Encore.Core.Dto;

namespace NetSteps.Taxes.Common.Models
{
  /// <summary>
  /// The TaxCacheKey interface.
  /// 
  /// </summary>
  [DTO]
  public interface ITaxCacheKey
  {
    /// <summary>
    /// Gets or sets a value indicating whether is detailed key.
    /// 
    /// </summary>
    bool IsDetailedKey { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether use province.
    /// 
    /// </summary>
    bool UseProvince { get; set; }

    /// <summary>
    /// Gets or sets the country id.
    /// 
    /// </summary>
    int CountryID { get; set; }

    /// <summary>
    /// Gets or sets the postal code.
    /// 
    /// </summary>
    string PostalCode { get; set; }

    /// <summary>
    /// Gets or sets the state abbr.
    /// 
    /// </summary>
    string StateAbbr { get; set; }

    /// <summary>
    /// Gets or sets the county.
    /// 
    /// </summary>
    string County { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// 
    /// </summary>
    string City { get; set; }
  }
}
