using System.Collections.Generic;

namespace NetSteps.Taxes.Common.Models
{
  /// <summary>
  /// Common interface for TaxDataSource.
  /// 
  /// </summary>
  public interface ITaxDataSource
  {
    /// <summary>
    /// The TaxDataSourceID for this TaxDataSource.
    /// 
    /// </summary>
    short TaxDataSourceID { get; set; }

    /// <summary>
    /// The Name for this TaxDataSource.
    /// 
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// The TermName for this TaxDataSource.
    /// 
    /// </summary>
    string TermName { get; set; }

    /// <summary>
    /// The Description for this TaxDataSource.
    /// 
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// The Active for this TaxDataSource.
    /// 
    /// </summary>
    bool Active { get; set; }

    /// <summary>
    /// The TaxCaches for this TaxDataSource.
    /// 
    /// </summary>
    IEnumerable<ITaxCache> TaxCaches { get; }

    /// <summary>
    /// Adds an <see cref="T:NetSteps.Taxes.Common.Models.ITaxCache"/> to the TaxCaches collection.
    /// 
    /// </summary>
    /// <param name="item">The <see cref="T:NetSteps.Taxes.Common.Models.ITaxCache"/> to add.</param><requires exception="T:System.ArgumentNullException" csharp="item != null" vb="item <> Nothing">item != null</requires><exception cref="T:System.ArgumentNullException">item == null</exception>
    void AddTaxCache(ITaxCache item);

    /// <summary>
    /// Removes an <see cref="T:NetSteps.Taxes.Common.Models.ITaxCache"/> from the TaxCaches collection.
    /// 
    /// </summary>
    /// <param name="item">The <see cref="T:NetSteps.Taxes.Common.Models.ITaxCache"/> to remove.</param><requires exception="T:System.ArgumentNullException" csharp="item != null" vb="item <> Nothing">item != null</requires><exception cref="T:System.ArgumentNullException">item == null</exception>
    void RemoveTaxCache(ITaxCache item);
  }
}
