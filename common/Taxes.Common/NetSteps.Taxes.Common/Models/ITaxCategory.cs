namespace NetSteps.Taxes.Common.Models
{
  /// <summary>
  /// Common interface for TaxCategory.
  /// 
  /// </summary>
  public interface ITaxCategory
  {
    /// <summary>
    /// The TaxCategoryID for this TaxCategory.
    /// 
    /// </summary>
    int TaxCategoryID { get; set; }

    /// <summary>
    /// The Name for this TaxCategory.
    /// 
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// The TermName for this TaxCategory.
    /// 
    /// </summary>
    string TermName { get; set; }

    /// <summary>
    /// The Description for this TaxCategory.
    /// 
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// The Active for this TaxCategory.
    /// 
    /// </summary>
    bool Active { get; set; }

    /// <summary>
    /// The IsDefault for this TaxCategory.
    /// 
    /// </summary>
    string IsDefault { get; set; }
  }
}
